using System;
using IDAL;
using System.Collections;
using System.Collections.Generic;
using IBL.BO;
using IBL;
using static IBL.BO.Enum;
using DalObject;
using BL.IBL.BO;
using System.Linq;
namespace BL
{
    public partial class BL
    {
        /// <summary>
        /// Performing logical tests on the recieved parcel and coverting the parcel fields in the dalObject
        /// to the parcel fields in the BL, and placing parcel into the dal list of drones.
        /// </summary>
        /// <param name="newParcel">The new parcel</param>
        public void AddParcel(Parcel newParcel)
        {
            if ((Math.Round(Math.Floor(Math.Log10(newParcel.SenderId.Id))) + 1) != 9)//if name inputted is not 9 digits long
                throw new InvalidInputException("The identification number of sender should be 9 digits long\n");
            if ((Math.Round(Math.Floor(Math.Log10(newParcel.TargetId.Id))) + 1) != 9)//if name inputted is not 9 digits long
                throw new InvalidInputException("The identification number of target should be 9 digits long\n");
            if (newParcel.Weight != (WeightCategories)1 && newParcel.Weight != (WeightCategories)2 && newParcel.Weight != (WeightCategories)3)//if 1,2 or 3 werent inputted
                throw new InvalidInputException("You need to select 1- for Easy 2- for Medium 3- for Heavy\n");
            if (newParcel.Priority != (Priorities)1 && newParcel.Priority != (Priorities)2 && newParcel.Priority != (Priorities)3)//if 1,2 or 3 were inputted
                throw new InvalidInputException("You need to select 1- for Normal 2- for Fast 3- for Emergency\n");
            //updating times
            newParcel.Requested = DateTime.Now;
            newParcel.Scheduled = DateTime.MinValue;
            newParcel.PickedUp = DateTime.MinValue;
            newParcel.Delivered = DateTime.MinValue;
            newParcel.DroneParcel = null;
            try
            {
                //converting BL parcel to dal
                IDAL.DO.Parcel tempParcel = new();
                newParcel.CopyPropertiesTo(tempParcel);
                dal.AddParcel(tempParcel);//adding to parcel array in dal
            }
            catch (IDAL.DO.ItemExistsException ex)
            {
                throw new FailedToAddException("The parcel already exists.\n", ex);
            }
        }

        /// <summary>
        /// Displays a specific parcel, by converting parcel to BL an filling the missing fields.
        /// </summary>
        /// <param name="parcelId">Id of parcel</param>
        /// <returns>Parcel</returns>
        public Parcel DisplayParcel(int parcelId)//תצוגת חבילה
        {
            Parcel blParcel = new();
            try
            {
                IDAL.DO.Parcel dalParcel = dal.FindParcel(parcelId);//finding parcel
                dalParcel.CopyPropertiesTo(blParcel);//converting to BL
                Customer target = DisplayCustomer(dalParcel.TargetId);//finding the target who will recieve parcel
                target.CopyPropertiesTo(blParcel.TargetId);//converting to BL
                Customer sender = DisplayCustomer(dalParcel.SenderId);//finding the sender who sends the parcel
                sender.CopyPropertiesTo(blParcel.SenderId);//converting to BL
                if (dalParcel.DroneId == 0)//if parcel isnt assigned to a drone
                    blParcel.DroneParcel = default;
                else
                {
                    Drone drone = DisplayDrone(dalParcel.DroneId);//finding its assogned drone
                    drone.CopyPropertiesTo(blParcel.DroneParcel);//converting to BL
                }
            }
            catch (IDAL.DO.ItemDoesNotExistException ex)
            {
                throw new FailedDisplayException("The Id does not exist.\n", ex);
            }
            return blParcel;
        }

        /// <summary>
        /// Converting BL list to dal and updating the parcel state, then adding to parcelToList.
        /// </summary>
        /// <returns>List of parcels</returns>
        public IEnumerable<ParcelToList> ListViewParcels()
        {
            Parcel tempParcel = new();
            ParcelToList tempParcelToList = new();
            List<ParcelToList> parcelToLists = new List<ParcelToList>();
            foreach (var indexOfParcels in dal.GetAllParcels())//goes through dals list of parcels
            {
                tempParcel = DisplayParcel(indexOfParcels.Id);//finding parcel
                tempParcel.CopyPropertiesTo(tempParcelToList);//converting to parcelToList
                if (tempParcel.Delivered != DateTime.MinValue)//if parcel was delivered
                    tempParcelToList.StateOfParcel = (ParcelState)4;//state -> provided
                else
                {
                    if (tempParcel.Scheduled != DateTime.MinValue)//if parcel was picked up by drone
                        tempParcelToList.StateOfParcel = (ParcelState)3;//state -> picked up
                    else
                    {
                        if (tempParcel.PickedUp != DateTime.MinValue)//if if parcel was assigned to drone
                            tempParcelToList.StateOfParcel = (ParcelState)2;//state -> paired
                        else//if parcel was requested
                            tempParcelToList.StateOfParcel = (ParcelState)1;//state -> created
                    }
                }
                parcelToLists.Add(tempParcelToList);

            }
            return parcelToLists;
        }

        /// <summary>
        /// Finds parcels that are not assigned to a drone.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ParcelToList> ParcelWithNoDrone()
        {
            List<ParcelToList> parcelToList = new();
            foreach (var indexOfParcelToList in ListViewParcels())
            {
                if (indexOfParcelToList.StateOfParcel == (ParcelState)1)
                    parcelToList.Add(indexOfParcelToList);
            }
            return parcelToList;
        }

        public void UpdateAssignParcelToDrone(int droneId)
        {
            dal.GetAllDrones().First(item => item.Id == droneId);//לברר
            Drone drone = DisplayDrone(droneId);
            if(drone.DroneStatus == (DroneStatuses)1)
            {

            }
        }
        public void UpdateParcelDeliveryToCustomer(int droneId)
        {
            dal.GetAllDrones().First(item => item.Id == droneId);//לברר
            Drone drone = DisplayDrone(droneId);//finding drone with given id
            Parcel parcel = DisplayParcel(drone.ParcelInTransfer.Id);//finding parcel that is assigned to this drone
            if (drone.ParcelInTransfer.ParcelState == true)//if parcel is delivered
            {
                //finding distance between original location of drone to the location of its destination
                int distance = (int)Distance.Haversine
                    (drone.ParcelInTransfer.CollectionLocation.Longitude, drone.ParcelInTransfer.CollectionLocation.Latitude,
                    drone.ParcelInTransfer.DeliveryDestination.Longitude, drone.ParcelInTransfer.DeliveryDestination.Latitude);
                //battery is measured by the distance the drone did and the amount of battery that goes down according to the parcel weight
                drone.Battery -= (int)(distance * elecUse[Weight(drone.MaxWeight)]);
                drone.CurrentLocation = drone.ParcelInTransfer.DeliveryDestination;//updating location to destination location
                drone.DroneStatus = (DroneStatuses)1;//updating status of drone to be available
                parcel.Delivered = DateTime.Now;//time of delivery is present time
            }
            else
                throw new FailedToDeliverParcel("Drone could not deliver parcel.\n");
        }

        public void UpdateParcelCollectionByDrone(int droneId)
        {
            dal.GetAllDrones().First(item => item.Id == droneId);//לברר
            Drone drone = DisplayDrone(droneId);//finding drone with given id
            Parcel parcel = DisplayParcel(drone.ParcelInTransfer.Id);//finding parcel that is assigned to this drone
            if (parcel.PickedUp != DateTime.MinValue)
            {
                //finding distance between original location of drone to the location of its destination
                int distance = (int)Distance.Haversine
                    (drone.CurrentLocation.Longitude, drone.CurrentLocation.Latitude,
                    drone.ParcelInTransfer.CollectionLocation.Longitude, drone.ParcelInTransfer.CollectionLocation.Latitude);
                //battery is measured by the distance the drone did and the amount of battery that goes down according to the parcel weight
                drone.Battery -= (int)(distance * elecUse[Weight(drone.MaxWeight)]);
                drone.CurrentLocation = drone.ParcelInTransfer.CollectionLocation;//updating location to sender location
                parcel.PickedUp = DateTime.Now;//time of picking up is present time
            }
            else
                throw new FailedToCollectParcel("Parcel could not be collected by drone.\n");
        }
    }
}
