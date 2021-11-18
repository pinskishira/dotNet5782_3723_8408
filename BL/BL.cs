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
        //public BL()
        //{
        //    dalObject.GetAllDrones().CopyPropertiesToIEnumerable(BlDrones);
        //    Random rand = new Random();
        //    double[] elecUse = dalObject.electricityUse();
        //    double Available = elecUse[0];
        //    double LightWeight = elecUse[1];
        //    double MediumWeight = elecUse[2];
        //    double HeavyWeight = elecUse[3];
        //    double DroneLoadingRate = elecUse[4];
        //    dalObject.GetAllParcels().First
        //    foreach (var indexOfDrones in BlDrones)
        //    {
        //        bool flag = true;
        //        foreach (var indexOfParcels in dalObject.GetAllParcels())//going over the parcels
        //        {
        //            if (indexOfParcels.DroneId == indexOfDrones.Id && indexOfParcels.Delivered == DateTime.MinValue)//if parcel was paired but not delivered
        //            {
        //                indexOfDrones.DroneStatus = (DroneStatuses)3;
        //                if (indexOfParcels.Scheduled != DateTime.MinValue && indexOfParcels.PickedUp == DateTime.MinValue)//if parcel was paired but not picked up
        //                {
        //                    ///לשנות את זה
        //                    indexOfDrones.CurrentLocation.Longitude = 0;
        //                    indexOfDrones.CurrentLocation.Latitude = 1;
        //                }
        //                if (indexOfParcels.PickedUp != DateTime.MinValue)//if parcel was picked up but not delivered 
        //                {
        //                    ///לשנות את זה
        //                    indexOfDrones.CurrentLocation.Longitude = 0;
        //                    indexOfDrones.CurrentLocation.Latitude = 1;
        //                }
        //                ///לשנות את זה
        //                indexOfDrones.Battery = 0;
        //                flag = false;
        //                break;
        //            }
        //        }
        //        if (flag == true)//if drone is not doing a delivery
        //            indexOfDrones.DroneStatus = (DroneStatuses)rand.Next(1, 3);
        //        if (indexOfDrones.DroneStatus == (DroneStatuses)2)
        //        {
        //            List<IDAL.DO.Station> tempStations = (List<IDAL.DO.Station>)dalObject.GetAllStations();

        //        }

        //    }
        //}

        public void AddStation(Station newStation)
        {
            if ((Math.Round(Math.Floor(Math.Log10(newStation.Id))) + 1) != 4)
                throw new InvalidInputException("The identification number should be 4 digits long\n");
            if (newStation.NameOfStation == "\n")
                throw new InvalidInputException("You have to enter a valid name, with letters\n");
            if (newStation.StationLocation.Longitude < -180 || newStation.StationLocation.Longitude > 180)
                throw new InvalidInputException("The longitude is not valid, enter a longitude point between -180 and 180\n");
            if (newStation.StationLocation.Latitude < -90 || newStation.StationLocation.Latitude > 90)
                throw new InvalidInputException("The Latitude is not valid, enter a Latitude point between -90 and 90\n");
            if (newStation.AvailableChargeSlots < 0)
                throw new InvalidInputException("The number of charging stations of the station is less than 0\n");
            try
            {
                IDAL.DO.Station tempStation = new();
                newStation.CopyPropertiesTo(tempStation);
                dalObject.AddStation(tempStation);
            }
            catch (IDAL.DO.ItemExistsException ex)
            {
                throw new FailedToAddException("The station already exists.\n", ex);
            }
        }

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
            IDAL.DO.Station newStation = dalObject.FindStation(stationNumber);//מוצא את התחנה לפי המספר מזהה שהמשתמש הכניס
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
                dalObject.AddDroneCharge(tempDroneCharge);//שולח להוספה את הרחפן בטעינה
                IDAL.DO.Drone tempDrone = new();
                newDrone.CopyPropertiesTo(tempDrone);
                dalObject.AddDrone(tempDrone);//שולח להוספה את הרחפן
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
            if ((Math.Round(Math.Floor(Math.Log10(newCustomer.Id))) + 1) != 9)//if name inputted is not 9 digits long
                throw new InvalidInputException("The identification number should be 9 digits long\n");
            if (newCustomer.Name == "\n")
                throw new InvalidInputException("You have to enter a valid name, with letters\n");
            if (newCustomer.Phone.Length != 10)
                throw new InvalidInputException("You have to enter a valid phone, with 10 digits\n");
            if (newCustomer.CustomerLocation.Longitude < -180 || newCustomer.CustomerLocation.Longitude > 180)
                throw new InvalidInputException("The longitude is not valid, enter a longitude point between -180 and 1800\n");
            if (newCustomer.CustomerLocation.Latitude < -90 || newCustomer.CustomerLocation.Latitude > 90)
                throw new InvalidInputException("The Latitude is not valid, enter a Latitude point between -90 and 90\n");
            try
            {
                IDAL.DO.Customer tempCustomer = new();
                newCustomer.CopyPropertiesTo(tempCustomer);
                dalObject.AddCustomer(tempCustomer);
            }
            catch (IDAL.DO.ItemExistsException ex)
            {
                throw new FailedToAddException("The customer already exists.\n", ex);
            }
        }

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
                dalObject.AddParcel(tempParcel);
            }
            catch (IDAL.DO.ItemExistsException ex)
            {
                throw new FailedToAddException("The parcel already exists.\n", ex);
            }
        }
        public Drone DisplayDrone(int droneId)
        {
            DroneToList tempDroneToList = BlDrones.Find(item => item.Id == droneId);
            if (tempDroneToList == default)
                throw new FailedDisplayException("The ID number does not exist\n");
            Drone dalDrone = new();
            dalDrone.Id = tempDroneToList.Id;
            dalDrone.Model = tempDroneToList.Model;
            dalDrone.MaxWeight = tempDroneToList.MaxWeight;
            dalDrone.Battery = tempDroneToList.Battery;
            dalDrone.DroneStatus = tempDroneToList.DroneStatus;
            dalDrone.CurrentLocation = tempDroneToList.CurrentLocation;
            if (tempDroneToList.ParcelNumInTransfer == 0)
                dalDrone.ParcelInTransfer = default;
            else
            {
                Parcel tempParcel = DisplayParcel(tempDroneToList.ParcelNumInTransfer);
                ParcelInTransfer tempParcelInTransfer = new();
                tempParcel.CopyPropertiesTo(tempParcelInTransfer);
                Customer Sender = DisplayCustomer(tempParcelInTransfer.Sender.Id);
                Customer Target = DisplayCustomer(tempParcelInTransfer.Target.Id);
                tempParcelInTransfer.CollectionLocation = Sender.CustomerLocation;
                tempParcelInTransfer.DeliveryDestination = Target.CustomerLocation;
                if (tempParcel.PickedUp == DateTime.MinValue)
                    tempParcelInTransfer.ParcelState = true;
                else
                    tempParcelInTransfer.ParcelState = false;
                tempParcelInTransfer.TransportDistance = Distance.Haversine
                (Sender.CustomerLocation.Latitude, Sender.CustomerLocation.Longitude, Target.CustomerLocation.Latitude, Target.CustomerLocation.Longitude);
            }

            return;
        }
        public Station DisplayStation(int stationId)
        {
            Station blStation = new();
            try
            {
                IDAL.DO.Station dalStation = dalObject.FindStation(stationId);
                dalStation.CopyPropertiesTo(blStation);
                foreach (var indexOfDroneCharges in dalObject.GetAllDroneCharges())
                {
                    if (indexOfDroneCharges.StationId == stationId)
                    {
                        DroneInCharging tempDroneInCharging = new();
                        tempDroneInCharging.Id = indexOfDroneCharges.DroneId;
                        DroneToList tempDroneToList = BlDrones.Find(indexDroneToList => indexDroneToList.Id == indexOfDroneCharges.DroneId);
                        if (tempDroneToList == default)
                            throw new FailedDisplayException("The Id number does not exist. \n");
                        tempDroneInCharging.Battery = tempDroneToList.Battery;
                        blStation.DronesInCharging.Add(tempDroneInCharging);
                    }
                }
            }
            catch (IDAL.DO.ItemDoesNotExistException ex)
            {
                throw new FailedDisplayException("The Id does not exist.\n", ex);
            }
            return blStation;
        }
        public Parcel DisplayParcel(int parcelId)
        {
            Parcel blParcel = new();
            try
            {
                IDAL.DO.Parcel dalParcel = dalObject.FindParcel(parcelId);
                dalParcel.CopyPropertiesTo(blParcel);
                foreach (var indexOfCustomers in dalObject.GetAllCustomers())
                {
                    if (indexOfCustomers.Id == blParcel.TargetId.Id)
                        blParcel.TargetId.Name = indexOfCustomers.Name;
                    //למלאות שם של שולח
                    DroneInParcel tempDroneInParcel = new();
                    tempDroneInParcel.Id = dalParcel.DroneId;
                    DroneToList tempDroneToList = BlDrones.Find(indexDroneToList => indexDroneToList.Id == tempDroneInParcel.Id);
                    if (tempDroneToList == default)
                        throw new FailedDisplayException("The Id number does not exist. \n");
                    tempDroneInParcel.Battery = tempDroneToList.Battery;
                    tempDroneInParcel.CurrentLocation = tempDroneToList.CurrentLocation;
                    blParcel.DroneParcel = tempDroneInParcel;
                }
  
            }
            catch (IDAL.DO.ItemDoesNotExistException ex)
            {
                throw new FailedDisplayException("The Id does not exist.\n", ex);
            }
            return blParcel;
        }
        public Customer DisplayCustomer(int customerId)
        {
            Customer blCustomer = new();
            try
            {
                IDAL.DO.Customer dalCustomer = dalObject.FindCustomer(customerId);
                dalCustomer.CopyPropertiesTo(blCustomer);
                foreach (var indexOfParcels in dalObject.GetAllParcels())
                {
                    if(indexOfParcels.SenderId==blCustomer.Id)
                    {
                        ParcelAtCustomer tempParcelAtCustomer = new();
                        indexOfParcels.CopyPropertiesTo(tempParcelAtCustomer);
                        if (indexOfParcels.Requested != DateTime.MinValue)//לעדכן את זה!!
                        {
                            tempParcelAtCustomer.StateOfParcel = (ParcelState)4;
                        }
                        else
                        {
                            tempParcelAtCustomer.StateOfParcel = (ParcelState)4;
                        }

                    }
                    //if (indexOfParcels.TargetId == customerId)
                    //{
                    //    ParcelAtCustomer tempParcelAtCustomer = new();
                    //    indexOfParcels.CopyPropertiesTo(tempParcelAtCustomer);
                    //    tempParcelAtCustomer.Id = indexOfParcels.TargetId;
                    //    //tempParcelAtCustomer.Weight = (WeightCategories)(IDAL.DO.WeightCategories)indexOfParcels.Weight;
                    //    //tempParcelAtCustomer.Priority = (Priorities)(IDAL.DO.Priorities)indexOfParcels.Priority;
                    //    if (indexOfParcels.Requested == DateTime.Now)
                    //        tempParcelAtCustomer.StateOfParcel = (ParcelState)1;
                    //    //else???
                    //    foreach (var indexOfcustomers in dalObject.GetAllCustomers())
                    //    {
                    //        if (indexOfcustomers.Id == indexOfParcels.TargetId)
                    //        {
                    //            blCustomer.ParcelsFromCustomers = indexOfcustomers.Id;
                    //        }
                    //    }
                    //}


                }
            }
            catch ()
            {

            }
            return blCustomer;
        }
        
    }
}
