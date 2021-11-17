﻿using System;
using IDAL;
using System.Collections;
using System.Collections.Generic;
using IBL.BO;
using static IBL.BO.Enum;
using DalObject;
using BL.IBL.BO;
using System.Linq;

namespace BL
{
    public class BL
    {
        static Random rand = new Random();
        IDal dalObject = new DalObject.DalObject();
        List<DroneToList> BlDrones = new();
        public BL()
        {
            dalObject.GetAllDrones().CopyPropertiesToIEnumerable(BlDrones);
            Random rand = new Random();
            double[] elecUse = dalObject.electricityUse();
            double Available = elecUse[0];
            double LightWeight = elecUse[1];
            double MediumWeight = elecUse[2];
            double HeavyWeight = elecUse[3];
            double DroneLoadingRate = elecUse[4];
            dalObject.GetAllParcels().First
            foreach (var indexOfDrones in BlDrones)
            {
                bool flag = true;
                foreach (var indexOfParcels in dalObject.GetAllParcels())//going over the parcels
                {
                    if (indexOfParcels.DroneId == indexOfDrones.Id && indexOfParcels.Delivered == DateTime.MinValue)//if parcel was paired but not delivered
                    {
                        indexOfDrones.DroneStatus = (DroneStatuses)3;
                        if (indexOfParcels.Scheduled != DateTime.MinValue && indexOfParcels.PickedUp == DateTime.MinValue)//if parcel was paired but not picked up
                        {
                            ///לשנות את זה
                            indexOfDrones.CurrentLocation.Longitude = 0;
                            indexOfDrones.CurrentLocation.Latitude = 1;
                        }
                        if (indexOfParcels.PickedUp != DateTime.MinValue)//if parcel was picked up but not delivered 
                        {
                            ///לשנות את זה
                            indexOfDrones.CurrentLocation.Longitude = 0;
                            indexOfDrones.CurrentLocation.Latitude = 1;
                        }
                        ///לשנות את זה
                        indexOfDrones.Battery = 0;
                        flag = false;
                        break;
                    }
                }
                if (flag == true)//if drone is not doing a delivery
                    indexOfDrones.DroneStatus = (DroneStatuses)rand.Next(1, 3);
                if (indexOfDrones.DroneStatus == (DroneStatuses)2)
                {
                    List<IDAL.DO.Station> tempStations = (List<IDAL.DO.Station>)dalObject.GetAllStations();
                    
                }

            }
        }

        public void AddStation(Station newStation)
        {
            try
            {
                if ((Math.Round(Math.Floor(Math.Log10(newStation.Id))) + 1) != 4) 
                    throw new InvalidInputException("The identification number should be 4 digits long\n");
                if (newStation.StationLocation.Longitude < -180 || newStation.StationLocation.Longitude > 180)
                    throw new InvalidInputException("The longitude of the station is less than 0\n");
                if (newStation.StationLocation.Latitude < -90 || newStation.StationLocation.Latitude > 90) 
                    throw new InvalidInputException("The latitude of the station is less than 0\n");
                if (newStation.AvailableChargeSlots < 0)
                    throw new InvalidInputException("The number of charging stations of the station is less than 0\n");
                IDAL.DO.Station tempStation = new();
                newStation.CopyPropertiesTo(tempStation);
                dalObject.AddStation(tempStation);
            }
            catch (IDAL.DO.ItemExistsException ex )
            {
                throw new FailedToAddException("The station already exists.\n", ex);
            }
        }

        public void AddDrone(Drone newDrone , int stationNumber)
        {
            try
            {
                if ((Math.Round(Math.Floor(Math.Log10(newDrone.Id))) + 1) != 5)//בודק שהמספר מזהה של הרחפן הוא 5 ספרות
                    throw new InvalidInputException("The identification number should be 5 digits long\n");
                if (newDrone.MaxWeight != (WeightCategories)1 && newDrone.MaxWeight != (WeightCategories)2 && newDrone.MaxWeight != (WeightCategories)3)
                    throw new InvalidInputException("You need to select 1- for Easy 2- for Medium 3- for Heavy\n");
                if ((Math.Round(Math.Floor(Math.Log10(stationNumber))) + 1) != 4)//בודק שהמספר תחנה הוא 4 ספרות
                    throw new InvalidInputException("The identification number should be 4 digits long\n");
                newDrone.Battery = rand.Next(20, 41);//מצב סוללה מוגרל בין 20 ל40
                newDrone.DroneStatus = (DroneStatuses)2;//מצב הרחפן יהיה בתחזוקה
                IDAL.DO.Station newStation = dalObject.FindStation(stationNumber);//מוצא את התחנה לפי המספר מזהה שהמשתמש הכניס
                newDrone.CurrentLocation.Longitude = newStation.Longitude;//מעדכן את הקוו אורך לפי הקו אורך של התחנה
                newDrone.CurrentLocation.Latitude = newStation.Latitude;//מעדכן את הקוו רוחב לפי הקו אורך של התחנה
                IDAL.DO.DroneCharge tempDroneCharge = new();//מעדכן שהרחפן בטעינה
                tempDroneCharge.DroneId = newDrone.Id;
                tempDroneCharge.StationId = stationNumber;
                IDAL.DO.Drone tempDrone = new();
                newDrone.CopyPropertiesTo(tempDrone);
                dalObject.AddDrone(tempDrone);//שולח להוספה את הרחפן
                dalObject.AddDroneCharge(tempDroneCharge);//שולח להוספה את הרחפן בטעינה
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
        public void AddCustomer(Customer newCustomer)
        {
            IDAL.DO.Customer tempCustomer = new();
            newCustomer.CopyPropertiesTo(tempCustomer);
            dalObject.AddCustomer(tempCustomer);
        }
        public void AddParcel(Parcel newParcel)
        {
            IDAL.DO.Parcel tempParcel = new();
            newParcel.CopyPropertiesTo(tempParcel);
            dalObject.AddParcel(tempParcel);
        }
    }

}
