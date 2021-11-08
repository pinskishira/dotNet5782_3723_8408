using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using DAL.IDAL.DO;
using IDAL;

namespace DalObject
{
    public partial class DalObject
    {
        /// <summary>
        /// Adding a new parcel to the array of parcels
        /// </summary>
        /// <param name="newParcel">The new parcel</param>
        public void AddParcel(Parcel newParcel)
        {
            if (!DataSource.Parcels.Exists(item => item.Id == newParcel.Id))
                throw new DataExceptions("The parcel already exists.\n");
            newParcel.Id = DataSource.Config.NextParcelNumber++;
            DataSource.Parcels.Add(newParcel);
        }
        /// <summary>
        /// Updating when parcel is collected by drone
        /// </summary>
        /// <param name="idParcel">Parcel who's collected by drone</param>
        public void UpdateParcelCollectionByDrone(int idParcel)
        {
            if (!DataSource.Parcels.Exists(item => item.Id == idParcel))
                throw new DataExceptions("The parcel does not exist.\n");
            int indexParcel = 0;
            Parcel newParcel = new();
            while (DataSource.Parcels[indexParcel].Id != idParcel)//finding parcel that was collected by drone
                indexParcel++;
            newParcel = DataSource.Parcels[indexParcel];
            newParcel.PickedUp = DateTime.Now;
            DataSource.Parcels[indexParcel] = newParcel;//updating date and time
        }
        /// <summary>
        /// Updating when parcel is delivered to customer
        /// </summary>
        /// <param name="idParcel">Parcel delivered to customer</param>
        public void UpdateParcelDeliveryToCustomer(int idParcel)
        {
            if (!DataSource.Parcels.Exists(item => item.Id == idParcel))
                throw new DataExceptions("The parcel does not exist.\n");
            Parcel newParcel = new();
            int indexParcel = 0/*,indexDrone=0*/;
            while (DataSource.Parcels[indexParcel].Id != idParcel)
                indexParcel++;
            newParcel = DataSource.Parcels[indexParcel];
            newParcel.Delivered = DateTime.Now;
            newParcel.DroneId = 0;
            DataSource.Parcels[indexParcel] = newParcel;
            DataSource.Config.NextParcelNumber--;//updating that theres one less parcel to deliver
            //while (DataSource.Drones[indexDrone].Id != DataSource.Parcels[indexParcel].DroneId)
            //    indexDrone++;
            //DataSource.Drones[indexDrone].Status = DroneStatuses.Available;//now drone is availbale
        }
        /// <summary>
        /// Finding requested parcel according to its ID name
        /// </summary>
        /// <param name="id">Wanted parcel</param>
        /// <returns></returns>
        public Parcel FindParcel(int id)
        {
            if (!DataSource.Parcels.Exists(item => item.Id == id))
                throw new DataExceptions("The parcel does not exist.\n");
            int indexFindParcel = 0;
            while (DataSource.Parcels[indexFindParcel].Id != id)//Going through parcels array
                indexFindParcel++;
            return DataSource.Parcels[indexFindParcel];
        }
        /// <summary>
        /// Gives a view of the array of parcels
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> GetAllParcels()
        {
            List<Parcel> tempParcels = new();
            foreach (var indexOfParcels in DataSource.Parcels)
            {
                tempParcels.Add(indexOfParcels);
            }
            return tempParcels;
        }
        /// <summary>
        /// Gives a view of the an array of parcels with no assigned drones
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> ParcelWithNoDrone()
        {
            List<Parcel> parcelNoDrone = new();
            foreach (var indexNoDrone in DataSource.Parcels)
            {
                if (indexNoDrone.DroneId == 0)
                    parcelNoDrone.Add(indexNoDrone);
            }
            return parcelNoDrone;
        }
    }
}
