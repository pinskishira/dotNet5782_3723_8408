using System;
using System.Collections.Generic;
using System.Linq;
using DO;

namespace Dal
{
    partial class DalObject
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
            int indexParcel = DataSource.Parcels.FindIndex(item => item.Id == idParcel);//finding parcel that was collected by drone
            if(indexParcel==-1)//checks if parcel exists
                throw new ItemDoesNotExistException("The parcel does not exist.\n");
            Parcel newParcel = DataSource.Parcels[indexParcel];
            newParcel.PickedUp = DateTime.Now;
            DataSource.Parcels[indexParcel] = newParcel;//updating date and time
        }

        public void UpdateParcelDeliveryToCustomer(int idParcel)
        {
            int indexParcel = DataSource.Parcels.FindIndex(item => item.Id == idParcel);//finding parcel
            if(indexParcel==-1)//checks if parcel exists
                throw new ItemDoesNotExistException("The parcel does not exist.\n");
            Parcel newParcel = DataSource.Parcels[indexParcel];
            newParcel.Delivered = DateTime.Now;
            newParcel.DroneId = 0;//not assigned to drone anymore
            DataSource.Parcels[indexParcel] = newParcel;
            DataSource.Config.NextParcelNumber--;//updating that theres one less parcel to deliver
        }

        public Parcel FindParcel(int id)
        {
            int indexParcel = DataSource.Parcels.FindIndex(item => item.Id == id);
            if(indexParcel==-1)//checks if parcel exists
                throw new ItemDoesNotExistException("The parcel does not exist.\n");
            return DataSource.Parcels[indexParcel];//finding parcel
        }

        public IEnumerable<Parcel> GetAllParcels(Predicate<Parcel> predicate = null)
        {
            return from itemParcel in DataSource.Parcels
                   where predicate == null ? true : predicate(itemParcel)
                   select itemParcel;
        }

        public void DeleteParcel(int id)
        {
            int indexOfParcel= DataSource.Parcels.FindIndex(item => item.Id == id);//checks if parcel exists
            if (indexOfParcel==-1)
                throw new ItemDoesNotExistException("The parcel does not exist.\n");
            DataSource.Parcels.RemoveAt(indexOfParcel);
        }
    }
}
