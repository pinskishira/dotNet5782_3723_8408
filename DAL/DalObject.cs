using System;
using IDAL.DO;

/// <summary>
/// DalObject defines arrays for the stations, drones, customers, parcels and drone charges and then updates them and fills them with data.
/// It also includes adding functions for all arrays as well as searching functions.
/// </summary>
namespace DalObject
{
    /// <summary>
    /// Functions that add and search on the arrays
    /// </summary>
    public class DalObject
    {
        /// <summary>
        /// Adding a new station to the array of stations
        /// </summary>
        /// <param name="newStation"></param>
        public static void AddStation(Station newStation)
        {
            DataSource.Stations[DataSource.Config.IndexStation++] = newStation;
        }

        /// <summary>
        /// Adding a new  drone to the array of drones
        /// </summary>
        /// <param name="newDrone">The new drone</param>
        public static void AddDrone(Drone newDrone)
        {
            DataSource.Drones[DataSource.Config.IndexDrone].Id = newDrone.Id;
            DataSource.Drones[DataSource.Config.IndexDrone].Model = newDrone.Model;
            DataSource.Drones[DataSource.Config.IndexDrone].Status = newDrone.Status;
            DataSource.Drones[DataSource.Config.IndexDrone].MaxWeight = newDrone.MaxWeight;
            DataSource.Config.IndexDrone++;//Promoting the index
        }
        /// <summary>
        /// Adding a new customer to the array of customers
        /// </summary>
        /// <param name="NewCustomer"></param>
        public static void AddCustomer(Customer NewCustomer)
        {
            DataSource.Customers[DataSource.Config.IndexCustomer].Id = NewCustomer.Id;
            DataSource.Customers[DataSource.Config.IndexCustomer].Name = NewCustomer.Name;
            DataSource.Customers[DataSource.Config.IndexCustomer].Phone = NewCustomer.Phone;
            DataSource.Customers[DataSource.Config.IndexCustomer].Longitude = NewCustomer.Longitude;
            DataSource.Customers[DataSource.Config.IndexCustomer].Latitude = NewCustomer.Latitude;
            DataSource.Config.IndexCustomer++;//Promoting the index
        }
        /// <summary>
        /// Adding a new parcel to the array of parcels
        /// </summary>
        /// <param name="NewParcel"></param>
        public static void AddParcel(Parcel NewParcel)
        {
            DataSource.Parcels[DataSource.Config.IndexParcel].Id = DataSource.Config.NextParcelNumber++;
            DataSource.Parcels[DataSource.Config.IndexParcel].SenderId = NewParcel.SenderId;
            DataSource.Parcels[DataSource.Config.IndexParcel].TargetId = NewParcel.TargetId;
            DataSource.Parcels[DataSource.Config.IndexParcel].Weight = NewParcel.Weight;
            DataSource.Parcels[DataSource.Config.IndexParcel].Priority = NewParcel.Priority;
            DataSource.Parcels[DataSource.Config.IndexParcel].Requested = NewParcel.Requested;
            DataSource.Parcels[DataSource.Config.IndexParcel].Scheduled = NewParcel.Scheduled;
            DataSource.Parcels[DataSource.Config.IndexParcel].Delivered = NewParcel.Delivered;
            DataSource.Parcels[DataSource.Config.IndexParcel].PickedUp = NewParcel.PickedUp;
            DataSource.Parcels[DataSource.Config.IndexParcel].DroneId = NewParcel.DroneId;
            DataSource.Config.IndexParcel++;//Promoting the index
        }
        public static void UpdateAssignParcelToDrone(int IdParcel, int IdDrone)
        {
            Drone AvailableDrone = new();
            Parcel NewParcel = new();
            for (int i = 0; i < DataSource.Config.IndexParcel; i++)
            {
                if (DataSource.Drones[i].Id == IdParcel)
                {
                    NewParcel = DataSource.Parcels[i];
                    break;
                }
            }
            for (int i = 0; i < DataSource.Config.IndexDrone; i++)
            {
                if (DataSource.Drones[i].Id == IdDrone)
                {
                    AvailableDrone = DataSource.Drones[i];
                    break;
                }
            }
            AvailableDrone.Status = DroneStatuses.Delivery;
            NewParcel.Scheduled = DateTime.Now;
            NewParcel.DroneId = AvailableDrone.Id;
        }

        public static void UpdateParcelCollectionByDrone(int IdParcel)
        {

            int indexParcel = 0;
            while (DataSource.Parcels[indexParcel].Id != IdParcel)
                indexParcel++;
            DataSource.Parcels[indexParcel].PickedUp = DateTime.Now;
        }

        public static void UpdateParcelDeliveryToCustomer(int IdParcel)
        {
            int index = 0;
            while (DataSource.Parcels[index].Id != IdParcel)
                index++;
            DataSource.Parcels[index].Delivered = DateTime.Now;
            DataSource.Config.NextParcelNumber--;
            index = 0;
            while (DataSource.Drones[index].Id != DataSource.Parcels[index].DroneId)
                index++;
            DataSource.Drones[index].Status = DroneStatuses.Available;
            DataSource.Parcels[index].DroneId = 0;
        }
        public static void UpdateSendDroneToChargingStation(int IdDrone, string nameStation)
        {
            DroneCharge NewDroneCharge = new();//drone with low battery will go be charged here
            int index = 0;
            while (DataSource.Drones[index].Id != IdDrone)
                index++;
            DataSource.Drones[index].Status = DroneStatuses.Maintenance;//saying that the drone is in maintanance and unavailable to deliver
            NewDroneCharge.DroneId = IdDrone;//putting id of low battery drone into its charging station
            index = 0;
            while (DataSource.Stations[index].Name != nameStation)
                index++;
            NewDroneCharge.StationId = DataSource.Stations[index].Id;
            AddDroneCharge(NewDroneCharge);//updating that a drone is charging 
            DataSource.Stations[index].ChargeSlots--;
        }

        /// <summary>
        /// Adding a new drone charge to the array of drone charges
        /// </summary>
        /// <param name="NewDroneCharge"></param>
        public static void AddDroneCharge(DroneCharge NewDroneCharge)
        {
            DataSource.DroneCharges[DataSource.Config.IndexDroneCharge].DroneId = NewDroneCharge.DroneId;
            DataSource.DroneCharges[DataSource.Config.IndexDroneCharge].StationId = NewDroneCharge.StationId;
            DataSource.Config.IndexDroneCharge++;//Promoting the index
        }
        public static void DroneReleaseFromChargingStation(int IdDrone)
        {
            int indexDC = 0;
            while (DataSource.DroneCharges[indexDC].DroneId != IdDrone)
                indexDC++;
            DataSource.DroneCharges[indexDC].DroneId = 0;
            DataSource.DroneCharges[indexDC].StationId = 0;
            int indexS = 0;
            while (DataSource.Stations[indexS].Id != DataSource.DroneCharges[indexDC].DroneId)
                indexS++;
            DataSource.Stations[indexS].ChargeSlots++;//increasing amount of places left to charge
        }

        /// <summary>
        /// Finding requested custmer according to its ID name
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Finding requested station according to its ID name
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Station FindStation(int id)
        {
            Station tempStation = new();
            for (int i = 0; i < DataSource.Config.IndexStation; i++)//Going through stations array
            {
                if (DataSource.Stations[i].Id == id)//finding requested station 
                {
                    return DataSource.Stations[i];//placing in a temporary array
                }
            }
            return tempStation;
        }
        /// <summary>
        /// Finding requested drone according to its ID name
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Finding requested parcel according to its ID name
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
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
        /// <summary>
        /// A function that returns an array of stationswhose load position is greater than 0
        /// </summary>
        /// <returns></returns>
        public static Station[] GetStationWithFreeSlots()
        {
            Station[] StationChargingSlot = new Station[DataSource.Config.IndexStation];
            int count = 0, i;
            for (i = 0; i < DataSource.Config.IndexStation; i++)
            {
                if (DataSource.Stations[i].ChargeSlots > 0)
                    StationChargingSlot[count] = DataSource.Stations[i];
                count++;
            }
            StationChargingSlot[i + 1].Id = -1;//Placing -1 in the last place in the array
            return StationChargingSlot;
        }




        /// <summary>
        /// Returns place of index in station array
        /// </summary>
        /// <returns></returns>
        public static int GetIndexStation()
        {
            return DataSource.Config.IndexStation;
        }





        /// <summary>
        /// Gives a view of the array of stations
        /// </summary>
        /// <returns></returns>
        public static Station[] GetAllStations()
        {
            Station[] list = new Station[DataSource.Config.IndexStation];//placing array in another array which will be sent back to main
            for (int i = 0; i < DataSource.Config.IndexStation; i++)
            {
                list[i] = DataSource.Stations[i];
            }
            return list;
        }
        /// <summary>
        /// Gives a view of the array of parcels
        /// </summary>
        /// <returns></returns>
        public static Parcel[] GetAllParcels()
        {
            Parcel[] ViewParcel = new Parcel[DataSource.Config.IndexParcel];
            for (int i = 0; i < DataSource.Config.IndexParcel; i++)
            {
                ViewParcel[i] = DataSource.Parcels[i];
            }
            return ViewParcel;
        }
        /// <summary>
        /// Gives a view of the array of drones
        /// </summary>
        /// <returns></returns>
        public static Drone[] GetAllDrones()
        {
            Drone[] ViewDrone = new Drone[DataSource.Config.IndexDrone];
            for (int i = 0; i < DataSource.Config.IndexDrone; i++)
            {
                ViewDrone[i] = DataSource.Drones[i];
            }
            return ViewDrone;
        }
        /// <summary>
        /// Gives a view of the array of customers
        /// </summary>
        /// <returns></returns>
        public static Customer[] GetAllCustomers()
        {
            Customer[] ViewCustomer = new Customer[DataSource.Config.IndexCustomer];
            for (int i = 0; i < DataSource.Config.IndexCustomer; i++)
            {
                ViewCustomer[i] = DataSource.Customers[i];
            }
            return ViewCustomer;
        }
        /// <summary>
        /// Gives a view of the an array of parcels with no assigned drones
        /// </summary>
        /// <returns></returns>
        public static Parcel[] ParcelWithNoDrone()
        {
            int count = 0;
            for (int i = 0; i < DataSource.Config.IndexParcel; i++)//counts how many parcels dont have an assigned drone
            {
                if (DataSource.Parcels[i].DroneId != 0)//if the drone ID doesnt equal 0
                    count++;
            }
            Parcel[] ViewParcel = new Parcel[count];
            for (int i = 0; i < count; i++)//places the parcels that dont have an assigned drone into an array
            {
                if (DataSource.Parcels[i].DroneId != 0)
                    ViewParcel[i] = DataSource.Parcels[i];
            }
            return ViewParcel;
        }
        /// <summary>
        /// Gives a view of the an array stations with available charging slots
        /// </summary>
        /// <returns></returns>
        public static Station[] AvailableChargingSlots()
        {
            int count = 0;
            for (int i = 0; i < DataSource.Config.IndexStation; i++)
            {
                if (DataSource.Stations[i].ChargeSlots > 0)//if the charge slots if greather than zero
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