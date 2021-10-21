using System;
using IDAL.DO;
Random rand = new Random(DateTime.Now.Millisecond);

//namespace DAL
//{
//    public class Class1
//    {
//    }
//}
namespace DalObject
{
    public class DataSource
    {
        static internal Drone[] Drones = new Drone[10];//defining an array of size 10 for the drones
        static internal Station[] Stations = new Station[5];//defining an array of size 5 for the stations
        static internal Customer[] Customers = new Customer[100];//defining an array of size 100 for the customers
        static internal Parcel[] Parcels = new Parcel[1000];//defining an array of size 10000 for the parcels
        static internal DroneCharge[] DroneCharges = new DroneCharge[100];//defining an array of size 1000 for the DroneCharges
        internal class Config
        {
            static internal int IndexDrone = 0, IndexStation = 0, IndexCustomer = 0, IndexParcel = 0, IndexDroneCharge = 0;
            static public int ParcelCounter = 0;
        }
        public static Random rand = new Random(DateTime.Now.Millisecond);
        public static void Initialize()
        {
            //updating 2 base stations
            for (int i = 0; i < 2; i++)
            {
                Stations[i].Id = rand.Next(1000, 10000);//name with 4 digits
                for (int j = 0; j < i; j++)
                {
                    while (Stations[i].Id == Stations[j].Id)
                        Stations[i].Id = rand.Next(1000, 10000);
                }
                //עדכון הקו אורך
                Stations[i].Longitude = rand.Next(100, 1000);//צריך לשנות את הטווחים
                                                             //עדכון הקו רוחב
                Stations[i].Latitude = rand.Next(100, 1000);//צריך לשנות את הטווחים
                                                            //עדכון מספר עמדות הטענה
                Stations[i].ChargeSlots = rand.Next(0, 30);
                DataSource.Config.IndexStation++;
            }
            //עדכון השמות
            Stations[0].Name = "Bayit Vegan";
            Stations[1].Name = "Givat Shaul";


            //עדכון 5 רחפנים
            for (int i = 0; i < 5; i++)
            {
                //עדכון המספר המזהה
                Drones[i].Id = rand.Next(10000, 100000);//name with 5 digits
                for (int j = 0; j < i; j++)
                {
                    while (Drones[i].Id == Drones[j].Id)
                        Drones[i].Id = rand.Next(10000, 100000);
                }
                //עדכון קטגורית משקל
                Drones[i].MaxWeight = (WeightCategories)rand.Next(0, 3);
                //עדכון מצב הרחפן
                Drones[i].Status = (DroneStatuses)rand.Next(0, 2);
                //עדכון מצב סוללה
                Drones[i].Battery = rand.Next(0, 101);//צריך לבדוק שזה בטוח הטווחים
                Drones[i].Model = "";//עדכון מודל
                DataSource.Config.IndexDrone++;
            }
            for (int i = 0; i < 10; i++)//עדכון 10 לקוחות
            {
                Customers[i].Id = rand.Next(100000000, 1000000000);//עדכון המספר המזהה בעזרת רנדום
                for (int j = 0; j < i; j++)
                {
                    while (Customers[i].Id == Customers[j].Id)
                        Customers[i].Id = rand.Next(100, 1000);
                }
                Customers[i].Longitude = rand.Next(100, 1000);//צריך לשנות את הטווחים//עדכון הקו אורך
                Customers[i].Latitude = rand.Next(100, 1000);//צריך לשנות את הטווחים //עדכון הקו רוחב
                DataSource.Config.IndexCustomer++;
            }
            //עדכון מודל
            Customers[0].Name = "Avital";
            Customers[1].Name = "Hadar";
            Customers[2].Name = "Ayala";
            Customers[3].Name = "Dasi";
            Customers[4].Name = "Moshe";
            Customers[5].Name = "Ayalet";
            Customers[6].Name = "David";
            Customers[7].Name = "Shira";
            Customers[8].Name = "Yosef";
            Customers[9].Name = "John";
            //עדכון מספר טלפון
            Customers[0].Phone = "058-6322431";
            Customers[1].Phone = "052-2230982";
            Customers[2].Phone = "050-6876398";
            Customers[3].Phone = "050-6561043";
            Customers[4].Phone = "050-2350982";
            Customers[5].Phone = "053-4456021";
            Customers[6].Phone = "055-2356731";
            Customers[7].Phone = "050-3782099";
            Customers[8].Phone = "050-4310431";
            Customers[9].Phone = "050-6929115";
            //עדכון 10 חבילות
            for (int i = 0; i < 10; i++)
            {
                Parcels[i].Id = rand.Next(1, 1000);//Update the ID number of the package
                Parcels[i].SenderId = rand.Next(100, 1000);//Update the ID number of the sender
                Parcels[i].Droneld = 0;//Update the ID number of the drone
                Parcels[i].TargetId = rand.Next(100000000, 1000000000);//Update the ID number of the customer
                for (int j = 0; j < i; j++)
                {
                    while (Parcels[i].Id == Parcels[j].Id)
                        Parcels[i].Id = rand.Next(100, 1000);
                }
                Parcels[i].Weight = (WeightCategories)rand.Next(0, 3);//Update the weight
                Parcels[i].Priority = (Priorities)rand.Next(0, 3);//Update the urgency of the shipment
                DateTime date = new DateTime(2021, rand.Next(1, 13), rand.Next(1, 30), rand.Next(1, 25), rand.Next(1, 61), rand.Next(1, 61));//Putting a random date and time
                Parcels[i].Requested = date;
                Parcels[i].Scheduled = date.AddMinutes(rand.Next(1, 61));//scheduling
                Parcels[i].Delivered = date.AddHours(rand.Next(1, 61));
                Parcels[i].PickedUp = date.AddDays(rand.Next(1, 15));
                DataSource.Config.IndexParcel++;
                DataSource.Config.ParcelCounter++;
            }
        }
    }
    public class DalObject
    {
        public static void AddStation(Station NewStation)//adding a new station to the array of stations
        {
            DataSource.Stations[DataSource.Config.IndexStation] = NewStation;
            DataSource.Config.IndexStation++;
        }
        public static void AddDrone(Drone NewDrone)//adding a new  drone to the array of drones
        {
            DataSource.Drones[DataSource.Config.IndexDrone] = NewDrone;
            DataSource.Config.IndexDrone++;
        }
        public static void AddCustomer(Customer NewCustomer)//adding a new customer to the array of customers
        {
            DataSource.Customers[DataSource.Config.IndexCustomer] = NewCustomer;
            DataSource.Config.IndexCustomer++;
        }
        public static void AddParcel(Parcel NewParcel)//adding a new parcel to the array of parcels
        {
            DataSource.Parcels[DataSource.Config.IndexParcel] = NewParcel;
            DataSource.Config.IndexParcel++;
        }
        public static void AddDroneCharge(DroneCharge NewDroneCharge)
        {
            DataSource.DroneCharges[DataSource.Config.IndexDroneCharge] = NewDroneCharge;
            DataSource.Config.IndexDroneCharge++;
        }
        //public static void DeleteDroneCharge(int IdDrone)
        //{
        //    static internal DroneCharge[] DroneCharges = new DroneCharge[100];
        //}
        public static Drone FindDroneAvailable()
        {
            Drone tempDrone = new();
            for (int i = 0; i < DataSource.Config.IndexDrone; i++)
            {
                if (DataSource.Drones[i].Status == DroneStatuses.Available)
                {
                    tempDrone = DataSource.Drones[i];
                    break;
                }
            }
            return tempDrone;
        }
        public static Station FindStation(int id)
        {
            Station tempStation = new();
            for (int i = 0; i < DataSource.Config.IndexStation; i++)
            {
                if (DataSource.Stations[i].Id == id)
                {
                    tempStation = DataSource.Stations[i];
                    break;
                }
            }
            return tempStation;
        }
        public static int FindStationId(string name)
        {
            Station tempStation = new();
            for (int i = 0; i < DataSource.Config.IndexStation; i++)
            {
                if (DataSource.Stations[i].Name == name)
                {
                    tempStation = DataSource.Stations[i];
                    break;
                }
            }
            return tempStation.Id;
        }
        public static Parcel FindParcel(int ID)
        {
            Parcel tempParcel = new();
            for (int i = 0; i < DataSource.Config.IndexParcel; i++)
            {
                if (DataSource.Drones[i].Id == ID)
                {
                    tempParcel = DataSource.Parcels[i];
                    break;
                }
            }
            return tempParcel;
        }
        public static Customer FindCustomer(int ID)
        {
            Customer tempCustomer = new();
            for (int i = 0; i < DataSource.Config.IndexCustomer; i++)
            {
                if (DataSource.Customers[i].Id == ID)
                {
                    tempCustomer = DataSource.Customers[i];
                    break;
                }
            }
            return tempCustomer;
        }
        public static Drone FindDrone(int ID)
        {
            Drone tempDrone = new();
            for (int i = 0; i < DataSource.Config.IndexDrone; i++)
            {
                if (DataSource.Drones[i].Id == ID)
                {
                    tempDrone = DataSource.Drones[i];
                    break;
                }
            }
            return tempDrone;
        }
        public static DroneCharge FindDroneCharge(int ID)
        {
            DroneCharge tempDroneCharge = new();
            for (int i = 0; i < DataSource.Config.IndexDroneCharge; i++)
            {
                if (DataSource.DroneCharges[i].DroneId == ID)
                {
                    tempDroneCharge = DataSource.DroneCharges[i];
                    break;
                }
            }
            return tempDroneCharge;
        }
        public static void UpdateParcelCounter(string flag)
        {
            if (flag == "Increase")
                DataSource.Config.ParcelCounter++;
            if (flag == "Decrease")
                DataSource.Config.ParcelCounter--;
        }
        public static Station[] GetStationWithFreeSlots()//פונקציה שמחזירה מערך של החנות שהעמדת טעינה שלהם גדולות מ0
        {
            Station[] StationChargingSlot = new Station[DataSource.Config.IndexStation];
            int count = 0, i;
            for (i = 0; i < DataSource.Config.IndexStation; i++)
            {
                if (DataSource.Stations[i].ChargeSlots > 0)
                    StationChargingSlot[count] = DataSource.Stations[i];
                count++;
            }
            StationChargingSlot[i + 1].Id = -1;
            return StationChargingSlot;
        }
        public static int GetIndexStation()
        {
            return DataSource.Config.IndexStation;
        }
        public static void IncreaseChargeSlots(Station UpdateStation)
        {
            UpdateStation.ChargeSlots++;
        }
        public static void DecreaseChargeSlots(Station UpdateStation)
        {
            UpdateStation.ChargeSlots--;
        }
        public static Station[] ListViewStation()
        {
            Station[] ViewStation = new Station[DataSource.Config.IndexStation - 1];
            for (int i = 0; i < DataSource.Config.IndexStation - 1; i++)
            {
                ViewStation[i] = DataSource.Stations[i];
            }
            return ViewStation;
        }
        public static Parcel[] ListViewParcel()
        {
            Parcel[] ViewParcel = new Parcel[DataSource.Config.IndexParcel - 1];
            for (int i = 0; i < DataSource.Config.IndexParcel - 1; i++)
            {
                ViewParcel[i] = DataSource.Parcels[i];
            }
            return ViewParcel;
        }
        public static Drone[] ListViewDrone()
        {
            Drone[] ViewDrone = new Drone[DataSource.Config.IndexDrone - 1];
            for (int i = 0; i < DataSource.Config.IndexDrone - 1; i++)
            {
                ViewDrone[i] = DataSource.Drones[i];
            }
            return ViewDrone;
        }
        public static Customer[] ListViewCustomer()
        {
            Customer[] ViewCustomer = new Customer[DataSource.Config.IndexCustomer - 1];
            for (int i = 0; i < DataSource.Config.IndexCustomer - 1; i++)
            {
                ViewCustomer[i] = DataSource.Customers[i];
            }
            return ViewCustomer;
        }
        public static Parcel[] ParcelWithNoDrone()
        {
            int count = 0;
            for (int i = 0; i < DataSource.Config.IndexParcel; i++)
            {
                if (DataSource.Parcels[i].Droneld != 0)
                    count++;
            }
            Parcel[] ViewParcel = new Parcel[count];
            for (int i = 0; i < count; i++)
            {
                if (DataSource.Parcels[i].Droneld != 0)
                    ViewParcel[i] = DataSource.Parcels[i];
            }
            return ViewParcel;
        }
        public static Station[] AvailableChargingSlots()
        {
            int count = 0;
            for (int i = 0; i < DataSource.Config.IndexStation; i++)
            {
                if (DataSource.Stations[i].ChargeSlots > 0)
                    count++;
            }
            Station[] ViewStation = new Station[count];
            for (int i = 0; i < count; i++)
            {
                if (DataSource.Stations[i].ChargeSlots > 0)
                    ViewStation[i] = DataSource.Stations[i];
            }
            return ViewStation;
           
        }
    }
}