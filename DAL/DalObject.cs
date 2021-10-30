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
            DataSource.Drones[DataSource.Config.IndexDrone++] = newDrone;
        }

        /// <summary>
        /// Adding a new customer to the array of customers
        /// </summary>
        /// <param name="NewCustomer"></param>
        public static void AddCustomer(Customer NewCustomer)
        {
            DataSource.Customers[DataSource.Config.IndexCustomer++] = NewCustomer;
        }

        /// <summary>
        /// Adding a new parcel to the array of parcels
        /// </summary>
        /// <param name="NewParcel"></param>
        public static void AddParcel(Parcel NewParcel)
        {
            DataSource.Parcels[DataSource.Config.IndexParcel]= NewParcel;
            DataSource.Parcels[DataSource.Config.IndexParcel].Id = DataSource.Config.NextParcelNumber++;
            DataSource.Config.IndexParcel++;//Promoting the index
        }

        public static void UpdateAssignParcelToDrone(int IdParcel, int IdDrone)
        {
            int indexAssign = 0;
            while (DataSource.Parcels[indexAssign].Id != IdParcel)
                indexAssign++;
            DataSource.Parcels[indexAssign].DroneId = IdDrone;
            DataSource.Parcels[indexAssign].Scheduled = DateTime.Now;
            while (DataSource.Drones[indexAssign].Id != IdDrone)
                indexAssign++;
            DataSource.Drones[indexAssign].Status = DroneStatuses.Delivery;
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
        public static Customer FindCustomer(int id)
        {
            int indexFindCustomer = 0;
            while (DataSource.Customers[indexFindCustomer].Id != id)
                indexFindCustomer++;
            return DataSource.Customers[indexFindCustomer];
        }

        /// <summary>
        /// Finding requested station according to its ID name
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Station FindStation(int id)
        {
            int indexFindStation = 0;
            while (DataSource.Stations[indexFindStation].Id != id)//Going through stations array
                indexFindStation++;
            return DataSource.Stations[indexFindStation];
        }

        /// <summary>
        /// Finding requested drone according to its ID name
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static Drone FindDrone(int id)
        {

            int indexFindDrone = 0;
            while (DataSource.Drones[indexFindDrone].Id != id)//Going through stations array
                indexFindDrone++;
            return DataSource.Drones[indexFindDrone];
        }

        /// <summary>
        /// Finding requested parcel according to its ID name
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static Parcel FindParcel(int id)
        {
            int indexFindParcel = 0;
            while (DataSource.Parcels[indexFindParcel].Id != id)//Going through stations array
                indexFindParcel++;
            return DataSource.Parcels[indexFindParcel];
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
                if (DataSource.Parcels[i].DroneId == 0)//if the drone ID doesnt equal 0
                    count++;
            }
            Parcel[] ViewParcel = new Parcel[count];
            count = 0;
            for (int i = 0; i < DataSource.Config.IndexParcel; i++)//places the parcels that dont have an assigned drone into an array
            {
                if (DataSource.Parcels[i].DroneId == 0)
                    ViewParcel[count++] = DataSource.Parcels[i];
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
            count = 0;
            for (int i = 0; i < DataSource.Config.IndexStation; i++)
            {
                if (DataSource.Stations[i].ChargeSlots > 0)
                    ViewStation[count++] = DataSource.Stations[i];
            }
            return ViewStation;
        }
    }
}