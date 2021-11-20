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
        /// Performing logical tests on the recieved new drone and the station its located in and coverting the 
        /// drone fields in the dalObject to the drone fields in the BL.
        /// </summary>
        /// <param name="newDrone">The new drone</param>
        /// <param name="stationNumber">Station where drone is located</param>
        public void AddDrone(Drone newDrone, int stationNumber)
        {
            if ((Math.Round(Math.Floor(Math.Log10(newDrone.Id))) + 1) != 5)//בודק שהמספר מזהה של הרחפן הוא 5 ספרות
                throw new InvalidInputException("The identification number should be 5 digits long\n");
            if (newDrone.Model.Length > 6)
                throw new InvalidInputException("The model number should be 6 digits long\n");
            if (newDrone.MaxWeight != (WeightCategories)1 && newDrone.MaxWeight != (WeightCategories)2 && newDrone.MaxWeight != (WeightCategories)3)
                throw new InvalidInputException("You need to select 1- for Easy 2- for Medium 3- for Heavy\n");
            if ((Math.Round(Math.Floor(Math.Log10(stationNumber))) + 1) != 4)//בודק שהמספר תחנה הוא 4 ספרות
                throw new InvalidInputException("The identification number should be 4 digits long\n");
            newDrone.Battery = rand.Next(20, 41);//מצב סוללה מוגרל בין 20 ל40
            newDrone.DroneStatus = (DroneStatuses)2;//מצב הרחפן יהיה בתחזוקה
            IDAL.DO.Station newStation = dal.FindStation(stationNumber);//מוצא את התחנה לפי המספר מזהה שהמשתמש הכניס
            newDrone.CurrentLocation.Longitude = newStation.Longitude;//מעדכן את הקוו אורך לפי הקו אורך של התחנה
            newDrone.CurrentLocation.Latitude = newStation.Latitude;//מעדכן את הקוו רוחב לפי הקו אורך של התחנה
            IDAL.DO.DroneCharge tempDroneCharge = new();//מעדכן שהרחפן בטעינה
            tempDroneCharge.DroneId = newDrone.Id;
            tempDroneCharge.StationId = stationNumber;
            DroneToList newDroneToList = new();
            newDroneToList.Id = newDrone.Id;
            newDroneToList.Model = newDrone.Model;
            newDroneToList.MaxWeight = newDrone.MaxWeight;
            newDroneToList.Battery = newDrone.Battery;
            newDroneToList.DroneStatus = newDrone.DroneStatus;
            newDroneToList.CurrentLocation = newDrone.CurrentLocation;
            newDroneToList.ParcelNumInTransfer = newDrone.ParcelInTransfer.Id;
            BlDrones.Add(newDroneToList);
            try
            {
                dal.AddDroneCharge(tempDroneCharge);//שולח להוספה את הרחפן בטעינה
                IDAL.DO.Drone tempDrone = new();
                newDrone.CopyPropertiesTo(tempDrone);
                dal.AddDrone(tempDrone);//שולח להוספה את הרחפן
            }
            catch (IDAL.DO.ItemExistsException ex)
            {
                throw new FailedToAddException("The drone already exists.\n", ex);
            }
            catch (IDAL.DO.ItemDoesNotExistException ex)
            {
                throw new FailedToAddException("The drone in charge does not exist.\n", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="droneId">Displaying drone</param>
        /// <returns></returns>
        public Drone DisplayDrone(int droneId)//תצוגת רחפן
        {
            DroneToList tempDroneToList = BlDrones.Find(item => item.Id == droneId);//חיפוש ברשימה של הרחפנים לפי מספר מזהה של הרחפן
            if (tempDroneToList == default)
                throw new FailedDisplayException("The ID number does not exist\n");
            Drone dalDrone = new();
            tempDroneToList.CopyPropertiesTo(dalDrone);//ממירים את רחפן ברשימה לרחפן רגיל
            if (tempDroneToList.ParcelNumInTransfer == 0)//לא שויך לחבילה רחפן
                dalDrone.ParcelInTransfer = default;
            else//כן שויך לחבילה רחפן
            {
                Parcel tempParcel = DisplayParcel(tempDroneToList.ParcelNumInTransfer);//מחפשים את החבילה לפי המספר המזהה
                ParcelInTransfer tempParcelInTransfer = new();
                tempParcel.CopyPropertiesTo(tempParcelInTransfer);//ממירים את החבילה לחבילה בהעברה
                Customer Sender = DisplayCustomer(tempParcelInTransfer.Sender.Id);//מוצאים את הלקוח לפי מספר מזהה
                Customer Target = DisplayCustomer(tempParcelInTransfer.Target.Id);//מוצאים את הלקוח לפי מספר מזהה
                tempParcelInTransfer.CollectionLocation = Sender.CustomerLocation;//מעדכנים את המרחק לפי המרחק של הלקוח
                tempParcelInTransfer.DeliveryDestination = Target.CustomerLocation;//מעדכנים את המרחק לפי המרחק של הלקוח
                if (tempParcel.PickedUp == DateTime.MinValue)//אם החבילה ממתינה עדיין לאיסוף
                    tempParcelInTransfer.ParcelState = false;
                else
                    tempParcelInTransfer.ParcelState = true;
                tempParcelInTransfer.TransportDistance = Distance.Haversine//חישוב המיקום הנוכחי של הרחפן
                (Sender.CustomerLocation.Latitude, Sender.CustomerLocation.Longitude, Target.CustomerLocation.Latitude, Target.CustomerLocation.Longitude);
                dalDrone.ParcelInTransfer = tempParcelInTransfer;
            }
            return dalDrone;
        }

        public IEnumerable<DroneToList> ListViewDrones()
        {
            return BlDrones;
        }

        public void UpdateDrone(int idDrone, string model)
        {
            dal.GetAllDrones().First(item => item.Id == idDrone);//לברר
            IDAL.DO.Drone drone = dal.GetAllDrones().First(indexDrones => indexDrones.Id == idDrone);
            drone.Model = model;
            dal.UpdateDrone(drone);
        }
        public void SendDroneToChargingStation(int idDrone)
        {
            try
            {
                Drone drone = DisplayDrone(idDrone);
                if (drone.DroneStatus != (DroneStatuses)1)//בדיקה אם הרחפן פנוי
                    throw new FailedSendDroneToChargingException("The drone is not available");
                IDAL.DO.Station station = smallestDistanceFromDrone(drone.CurrentLocation);
                if (station.Id == -1)
                    throw new FailedSendDroneToChargingException("There is no station with available charging stations");
                double batteryConsumption = Distance.Haversine
                    (drone.CurrentLocation.Longitude, drone.CurrentLocation.Latitude, station.Longitude, station.Latitude)*elecUse[0];
                if (batteryConsumption < drone.Battery)
                    throw new FailedSendDroneToChargingException("The drone does not have enough battery to go to the station");
                //סוללת הרחפן תעודכן לפי הזמן שהיא נסעה לתחנה בשביל להיטען
                drone.CurrentLocation.Longitude = station.Longitude;
                drone.CurrentLocation.Latitude = station.Latitude;
                drone.DroneStatus = (DroneStatuses)2;
                DroneToList droneToList = new();//לעדכן ברשימה של הרחפנים לרשימה את הרחפן הזה
                drone.CopyPropertiesTo(droneToList);
                droneToList.ParcelNumInTransfer = drone.ParcelInTransfer.Id;
                int indexDroneToList = BlDrones.FindIndex(indexOfDroneToList => indexOfDroneToList.Id == droneToList.Id);
                BlDrones[indexDroneToList] = droneToList;
                dal.UpdateSendDroneToChargingStation(drone.Id, station.Name);//גם מעדכן שהעמדות טעינה ירדו וגם מוסיף את הרחפן לרשימה של הרחפנים בטעינה
            }
            catch (IDAL.DO.ItemExistsException ex)
            {
                throw new FailedSendDroneToChargingException(ex.ToString(), ex);
            }
            catch (IDAL.DO.ItemDoesNotExistException ex)
            {
                throw new FailedSendDroneToChargingException(ex.ToString(), ex);
            }
        }

        public void DroneReleaseFromChargingStation(int idDrone)
        {

        }

        public IDAL.DO.Station smallestDistanceFromDrone(Location CurrentLocation)
        {
            double minDistance = double.PositiveInfinity;//starting with an unlimited value
            IDAL.DO.Station station = new();
            station.Id = -1;
            double tempDistance = -1;
            foreach (var indexOfStations in dal.GetAllStations())//goes through all the stations 
            {
                //calculating the distance between the sender and the station
                tempDistance = Distance.Haversine(indexOfStations.Longitude, indexOfStations.Latitude, CurrentLocation.Longitude, CurrentLocation.Latitude) ;
                if (tempDistance < minDistance && indexOfStations.ChargeSlots > 0)//compares which distance is smaller
                {
                    minDistance = tempDistance;
                    station = indexOfStations;
                }
            }
            return station;//returns closest station to sender
        }
    }
}
