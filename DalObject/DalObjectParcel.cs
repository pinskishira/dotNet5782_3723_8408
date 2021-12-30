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
            if (DataSource.Parcels.Exists(item => item.Id == newParcel.Id && !newParcel.DeletedParcel))//checks if parcel exists
                throw new ItemExistsException("The parcel already exists.\n");
            newParcel.Id = DataSource.Config.NextParcelNumber++;
            DataSource.Parcels.Add(newParcel);
        }

        public void UpdateParcelCollectionByDrone(int idParcel)
        {
            int indexParcel = CheckExistingParcel(idParcel);//finding parcel that was collected by drone
            Parcel newParcel = DataSource.Parcels[indexParcel];
            newParcel.PickedUp = DateTime.Now;
            DataSource.Parcels[indexParcel] = newParcel;//updating date and time
        }

        public void UpdateParcelDeliveryToCustomer(int idParcel)
        {
            int indexParcel = CheckExistingParcel(idParcel);//finding parcel
            Parcel newParcel = DataSource.Parcels[indexParcel];
            newParcel.Delivered = DateTime.Now;
            newParcel.DroneId = 0;//not assigned to drone anymore
            DataSource.Parcels[indexParcel] = newParcel;
            DataSource.Config.NextParcelNumber--;//updating that theres one less parcel to deliver
        }

        public Parcel FindParcel(int id)
        {
            int indexParcel = CheckExistingParcel(id);
            return DataSource.Parcels[indexParcel];//finding parcel
        }

        public IEnumerable<Parcel> GetAllParcels(Predicate<Parcel> predicate = null)
        {
            return from itemParcel in DataSource.Parcels
                   where predicate == null ? true : predicate(itemParcel)
                   select itemParcel;
        }

        private int CheckExistingParcel(int id)
        {
            int index = DataSource.Parcels.FindIndex(parcel => parcel.Id == id);
            if (index == -1)
                throw new ItemDoesNotExistException("No parcel found with this id");
            if (DataSource.Parcels[index].DeletedParcel)
                throw new ItemDoesNotExistException("This parcel is deleted");
            return index;
        }


        public void DeleteParcel(int id)
        {
            int indexOfParcel = CheckExistingParcel(id);//checks if parcel exists
            Parcel parcel = DataSource.Parcels[indexOfParcel];
            parcel.DeletedParcel = true;
            DataSource.Parcels[indexOfParcel] = parcel;
        }
    }
}
