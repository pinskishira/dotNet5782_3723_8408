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
        static internal Drone[] Drones = new Drone[10];
        static internal Station[] Stations = new Station[5];
        static internal Customer[] Customers = new Customer[100];
        static internal Parcel[] Parcels = new Parcel[1000];
        internal class Config
        {
            public static Random rand = new Random(DateTime.Now.Millisecond);
            static internal int indexDrone = 0, indexStation = 0, indexCustomer = 0, indexParcel = 0;
            public static void Initialize()
            {
                //עדכון 2 תחנות בסיס
                for (int i = 0; i < 2; i++)
                {
                    //עדכון המספר המזהה 
                    Stations[i].Id = i+1;
                    //עדכון הקו אורך
                    Stations[i].Longitude = rand.Next(100, 1000);//צריך לשנות את הטווחים
                    //עדכון הקו רוחב
                    Stations[i].Latitude = rand.Next(100, 1000);//צריך לשנות את הטווחים
                    //עדכון מספר עמדות הטענה
                    Stations[i].ChargeSlots = rand.Next(0, 30);
                    indexStation++;
                }
                //עדכון השמות
                Stations[0].Name = "Shira Pinski";
                Stations[1].Name = "Yirat Biton";


                //עדכון 5 רחפנים
                for (int i = 0; i < 5; i++)
                {
                    //עדכון המספר המזהה
                    Drones[i].Id = i+1;
                    //עדכון קטגורית משקל
                    Drones[i].MaxWeight = (WeightCategories)rand.Next(0, 3);
                    //עדכון מצב הרחפן
                    Drones[i].Status = (DroneStatuses)rand.Next(0, 3);
                    //עדכון מצב סוללה
                    Drones[i].Battery = rand.Next(0, 101);//צריך לבדוק שזה בטוח הטווחים
                    //עדכון מודל
                    Drones[i].Model = "Drone" + i+1;
                    indexDrone++;
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
                    indexCustomer++;
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
                    // עדכון המספר המזהה של החבילה
                    Parcels[i].Id = i + 1;
                    //עדכון המספר המזהה של השולח
                    Parcels[i].SenderId = i + 1;
                    //עדכון המספר המזהה של הרחפן
                    Parcels[i].Droneld = i + 1;
                    //עדכון המספר המזהה של הלקוח
                    Parcels[i].Id = rand.Next(100000000, 1000000000);
                    for (int j = 0; j < i; j++)
                    {
                        while (Parcels[i].Id == Parcels[j].Id)
                            Parcels[i].Id = rand.Next(100, 1000);
                    }
                    //עדכון קטגורית משקל
                    Parcels[i].Weight = (WeightCategories)rand.Next(0, 3);
                    //עדכון קטגורית דחיפות המשלוח
                    Parcels[i].Priority = (Priorities)rand.Next(0, 3);

                    DateTime date = new DateTime(2005, 12, 12, 9, 0, 0);////////צריך לתקן
                    //Parcels[i].Requested=rand.Next()
                    indexParcel++;
                }
            }
        }
    }
    public class DalObject
    {

        DalObject() { DataSource.Config.Initialize(); }
    }
}
