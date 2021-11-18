using System;
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
        public Station DisplayStation(int stationId) //תצוגת תחנת-בסיס
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
        public Parcel DisplayParcel(int parcelId)//תצוגת חבילה
        {
            Parcel blParcel = new();
            try
            {
                IDAL.DO.Parcel dalParcel = dalObject.FindParcel(parcelId);
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
        public Customer DisplayCustomer(int customerId)//תצוגת לקוח
        {
            Customer blCustomer = new();
            try
            {
                IDAL.DO.Customer dalCustomer = dalObject.FindCustomer(customerId);// מחפש את הלקוח לפי מספר מזהה
                dalCustomer.CopyPropertiesTo(blCustomer);//המרה-dal->bl
                foreach (var indexOfParcels in dalObject.GetAllParcels())//עובר על כל הרשימה של החבילות
                {
                    if (indexOfParcels.SenderId == blCustomer.Id || indexOfParcels.TargetId == blCustomer.Id)//אם הלקוח שאנחנו רוצים הוא או המוסר או המקבל את החבילה
                    {
                        ParcelAtCustomer parcelAtCustomer = new();
                        indexOfParcels.CopyPropertiesTo(parcelAtCustomer);// ממיר את החבילה// dal->bl
                        if (indexOfParcels.Scheduled != DateTime.MinValue)//החבילה שויכה
                        {
                            if (indexOfParcels.PickedUp != DateTime.MinValue)// אספו את החבילה
                            {
                                if (indexOfParcels.Delivered != DateTime.MinValue)//ההזמנה סופקה
                                    parcelAtCustomer.StateOfParcel = (ParcelState)4;
                                else
                                    parcelAtCustomer.StateOfParcel = (ParcelState)3;
                            }
                            else
                                parcelAtCustomer.StateOfParcel = (ParcelState)2;
                        }
                        else
                            parcelAtCustomer.StateOfParcel = (ParcelState)1;
                        parcelAtCustomer.SourceOrDestination.Id = blCustomer.Id;//מעדכן את פרטי מקור של החבילה
                        parcelAtCustomer.SourceOrDestination.Name = blCustomer.Name;//מעדכן את פרטי מקור של החבילה
                        if (indexOfParcels.SenderId == blCustomer.Id)//אם הלקוח שולח את החבילה
                            blCustomer.ParcelsFromCustomers.Add(parcelAtCustomer);
                        else//אם הלקוח מקבל את החבילה
                            blCustomer.ParcelsToCustomers.Add(parcelAtCustomer);
                    }
                }
            }
            catch (IDAL.DO.ItemDoesNotExistException ex)
            {
                throw new FailedDisplayException("The Id does not exist.\n", ex);
            }
            return blCustomer;
        }

        public IEnumerable<StationToList> ListViewStations()
        {
            Station tempStation = new();
            StationToList tempStationToList = new();
            List<StationToList> stationToList = new List<StationToList>();
            foreach (var indexOfStations in dalObject.GetAllStations())
            {
                tempStation = DisplayStation(indexOfStations.Id);
                tempStation.CopyPropertiesTo(tempStationToList);//
                foreach (var inCharging in tempStation.DronesInCharging)//בודק כמה רחפנים בטעינה וסופר
                    tempStationToList.UnavaialbleChargingSlots++;
                stationToList.Add(tempStationToList);
            }
            return stationToList;
        }


        public IEnumerable<ParcelToList> ListViewParcels()
        {
            Parcel tempParcel = new();
            ParcelToList tempParcelToList = new();
            List<ParcelToList> parcelToLists = new List<ParcelToList>();
            foreach (var indexOfParcels in dalObject.GetAllParcels())
            {
                tempParcel = DisplayParcel(indexOfParcels.Id);
                tempParcel.CopyPropertiesTo(tempParcelToList);

            }
            return parcelToLists;
        }
        public IEnumerable<DroneToList> ListViewDrones()
        {
            return BlDrones;
        }
        public IEnumerable<CustomerToList> ListViewCustomers()
        {
            Customer tempCustomer = new();
            CustomerToList tempCustomerToList = new();
            List<CustomerToList> customerToList = new();
            foreach (var indexCustomer in dalObject.GetAllCustomers())
            {
                tempCustomer = DisplayCustomer(indexCustomer.Id);//מביא את הלקוח לפי המספר המזהה
                tempCustomer.CopyPropertiesTo(tempCustomerToList);//המרה//dal->bll
                foreach (var parcelsFromCustomers in tempCustomer.ParcelsFromCustomers)//עובר על הרשימה של החבילות מהלקוח
                {
                    if (parcelsFromCustomers.StateOfParcel == (ParcelState)4)//אם החבילה סופקה
                        tempCustomerToList.ParcelsSentAndDelivered++;
                    else//אם החבילה לא סופקה
                        tempCustomerToList.ParcelsSentButNotDelivered++;
                }
                foreach (var parcelsToCustomers in tempCustomer.ParcelsToCustomers)//עובר על הרשימה של החבילות ללקוח
                {
                    if (parcelsToCustomers.StateOfParcel == (ParcelState)4)//אם הוא קיבל את החבילה
                        tempCustomerToList.RecievedParcels++;
                    else//אם החבילה בדרך
                        tempCustomerToList.ParcelsOnTheWayToCustomer++;
                }
                customerToList.Add(tempCustomerToList);
            }
            return customerToList;
        }
        public IEnumerable<ParcelToList> ParcelWithNoDrone()
        {
            List<ParcelToList> parcelToList = new();

            foreach (var indexOfParcels in ListViewParcels())
            {
                
            }

            return;
        }
        public IEnumerable<StationToList> GetStationWithFreeSlots()
        {
            List<StationToList> stationToList = new();
            foreach (var indexOfStation in ListViewStations())
            {
                if (indexOfStation.AvailableChargingSlots > 0)
                    stationToList.Add(indexOfStation);
            }
            return stationToList;
        }
    }
}
