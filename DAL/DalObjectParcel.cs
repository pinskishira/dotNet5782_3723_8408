using System;
using System.Collections.Generic;

using IDAL.DO;

namespace DalObject
{
    public partial class DalObject
    {
        public void AddParcel(Parcel newParcel)
        {
            if (DataSource.Parcels.Exists(item => item.Id == newParcel.Id))//checks if parcel exists
                throw new ItemExistsException("The parcel already exists.\n");
            newParcel.Id = DataSource.Config.NextParcelNumber++;
            DataSource.Parcels.Add(newParcel);
        }
        
        public void UpdateParcelCollectionByDrone(int idParcel)
        {
            if (!DataSource.Parcels.Exists(item => item.Id == idParcel))//checks if parcel exists
                throw new ItemDoesNotExistException("The parcel does not exist.\n");
            int indexParcel = DataSource.Parcels.FindIndex(item => item.Id == idParcel);//finding parcel that was collected by drone
            Parcel newParcel = DataSource.Parcels[indexParcel];
            newParcel.PickedUp = DateTime.Now;
            DataSource.Parcels[indexParcel] = newParcel;//updating date and time
        }
       
        public void UpdateParcelDeliveryToCustomer(int idParcel)
        {
            if (!DataSource.Parcels.Exists(item => item.Id == idParcel))//checks if parcel exists
                throw new ItemDoesNotExistException("The parcel does not exist.\n");
            int indexParcel = DataSource.Parcels.FindIndex(item => item.Id == idParcel);//finding parcel
            Parcel newParcel = DataSource.Parcels[indexParcel];
            newParcel.Delivered = DateTime.Now;
            newParcel.DroneId = 0;//not assigned to drone anymore
            DataSource.Parcels[indexParcel] = newParcel;
            DataSource.Config.NextParcelNumber--;//updating that theres one less parcel to deliver
        }

        public Parcel FindParcel(int id)
        {
            if (!DataSource.Parcels.Exists(item => item.Id == id))//checks if parcel exists
                throw new ItemDoesNotExistException("The parcel does not exist.\n");
            int indexFindParcel = DataSource.Parcels.FindIndex(item => item.Id == id);//finding parcel
            return DataSource.Parcels[indexFindParcel];
        }

        public IEnumerable<Parcel> GetAllParcels()
        {
            List<Parcel> tempParcels = new();
            foreach (var indexOfParcels in DataSource.Parcels)//going through parcels list
            {
                tempParcels.Add(indexOfParcels);//adding to list
            }
            return tempParcels;
        }

        public IEnumerable<Parcel> GetParcelWithNoDrone()
        {
            List<Parcel> parcelNoDrone = new();
            foreach (var indexNoDrone in DataSource.Parcels)//going through parcels list
            {
                if (indexNoDrone.DroneId == 0)//if parcel isnt assigned to drone
                    parcelNoDrone.Add(indexNoDrone);
            }
            return parcelNoDrone;
        }
    }
}
