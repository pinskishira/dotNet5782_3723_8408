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
        /// to the parcel fields in the BL.
        /// </summary>
        /// <param name="newParcel">The new parcel</param>
        public void AddParcel(Parcel newParcel)
        {
            if ((Math.Round(Math.Floor(Math.Log10(newParcel.SenderId.Id))) + 1) != 9)//if name inputted is not 9 digits long
                throw new InvalidInputException("The identification number of sender should be 9 digits long\n");
            if ((Math.Round(Math.Floor(Math.Log10(newParcel.TargetId.Id))) + 1) != 9)//if name inputted is not 9 digits long
                throw new InvalidInputException("The identification number of target should be 9 digits long\n");
            if (newParcel.Weight != (WeightCategories)1 && newParcel.Weight != (WeightCategories)2 && newParcel.Weight != (WeightCategories)3)
                throw new InvalidInputException("You need to select 1- for Easy 2- for Medium 3- for Heavy\n");
            if (newParcel.Priority != (Priorities)1 && newParcel.Priority != (Priorities)2 && newParcel.Priority != (Priorities)3)
                throw new InvalidInputException("You need to select 1- for Normal 2- for Fast 3- for Emergency\n");
            newParcel.Requested = DateTime.Now;
            newParcel.Scheduled = DateTime.MinValue;
            newParcel.PickedUp = DateTime.MinValue;
            newParcel.Delivered = DateTime.MinValue;
            newParcel.DroneParcel = null;
            try
            {
                IDAL.DO.Parcel tempParcel = new();
                newParcel.CopyPropertiesTo(tempParcel);
                dal.AddParcel(tempParcel);
            }
            catch (IDAL.DO.ItemExistsException ex)
            {
                throw new FailedToAddException("The parcel already exists.\n", ex);
            }
        }

        public Parcel DisplayParcel(int parcelId)//תצוגת חבילה
        {
            Parcel blParcel = new();
            try
            {
                IDAL.DO.Parcel dalParcel = dal.FindParcel(parcelId);
                dalParcel.CopyPropertiesTo(blParcel);
                Customer target = DisplayCustomer(dalParcel.TargetId);
                target.CopyPropertiesTo(blParcel.TargetId);
                Customer sender = DisplayCustomer(dalParcel.SenderId);
                sender.CopyPropertiesTo(blParcel.SenderId);
                if (dalParcel.DroneId == 0)
                    blParcel.DroneParcel = default;
                else
                {
                    Drone drone = DisplayDrone(dalParcel.DroneId);
                    drone.CopyPropertiesTo(blParcel.DroneParcel);
                }
            }
            catch (IDAL.DO.ItemDoesNotExistException ex)
            {
                throw new FailedDisplayException("The Id does not exist.\n", ex);
            }
            return blParcel;
        }

        public IEnumerable<ParcelToList> ListViewParcels()
        {
            Parcel tempParcel = new();
            ParcelToList tempParcelToList = new();
            List<ParcelToList> parcelToLists = new List<ParcelToList>();
            foreach (var indexOfParcels in dal.GetAllParcels())
            {
                tempParcel = DisplayParcel(indexOfParcels.Id);
                tempParcel.CopyPropertiesTo(tempParcelToList);
                if (tempParcel.Delivered != DateTime.MinValue)
                    tempParcelToList.StateOfParcel = (ParcelState)4;
                else
                {
                    if (tempParcel.Scheduled != DateTime.MinValue)
                        tempParcelToList.StateOfParcel = (ParcelState)3;
                    else
                    {
                        if (tempParcel.PickedUp != DateTime.MinValue)
                            tempParcelToList.StateOfParcel = (ParcelState)2;
                        else
                            tempParcelToList.StateOfParcel = (ParcelState)1;
                    }
                }
                parcelToLists.Add(tempParcelToList);

            }
            return parcelToLists;
        }

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
    }
}
