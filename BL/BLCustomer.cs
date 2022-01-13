using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using BO;
using static BO.Enum;

namespace BL
{
    partial class BL
    {

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomer(Customer newCustomer)
        {
            if ((Math.Round(Math.Floor(Math.Log10(newCustomer.Id))) + 1) != 9)//if id inputted is not 9 digits long
                throw new InvalidInputException("The identification number should be 9 digits long\n");
            if (newCustomer.Name == "\n")//if nothing was inputted as name for station
                throw new InvalidInputException("You have to enter a valid name, with letters\n");
            if (newCustomer.Phone.Length != 10)//if phone number isnt 10 digits
                throw new InvalidInputException("You have to enter a valid phone, with 10 digits\n");
            //if longitude isnt between 29.3 and 33.5 and latitude isnt between 33.7 and 36.3
            if (newCustomer.CustomerLocation.Longitude < 29.3 || newCustomer.CustomerLocation.Longitude > 33.5)
                throw new InvalidInputException("The longitude is not valid, enter a longitude point between 29.3 and 33.5\n");
            if (newCustomer.CustomerLocation.Latitude < 33.7 || newCustomer.CustomerLocation.Latitude > 36.3)
                throw new InvalidInputException("The Latitude is not valid, enter a Latitude point between 33.7 and 36.3\n");
            lock (dal)
            {
                try
                {
                    DO.Customer tempCustomer = new DO.Customer();
                    object obj = tempCustomer;
                    newCustomer.CopyPropertiesTo(obj);
                    tempCustomer = (DO.Customer)obj;
                    newCustomer.CopyPropertiesTo(tempCustomer);
                    tempCustomer.Longitude = newCustomer.CustomerLocation.Longitude;
                    tempCustomer.Latitude = newCustomer.CustomerLocation.Latitude;
                    dal.AddCustomer(tempCustomer);//adding to station list in dal
                }
                catch (DO.ItemExistsException ex)
                {
                    throw new FailedToAddException("ERROR.\n", ex);
                }
            }
        }

        private ParcelAtCustomer NewMethod(DO.Parcel indexOfParcels, Customer blCustomer)
        {
            lock (dal)
            {
                ParcelAtCustomer parcelAtCustomer = new ParcelAtCustomer();
                indexOfParcels.CopyPropertiesTo(parcelAtCustomer);// converting dal->bl
                                                                  //If the customer we want is either the sender or the recipient of the package
                if (indexOfParcels.SenderId == blCustomer.Id || indexOfParcels.TargetId == blCustomer.Id)
                {
                    if (indexOfParcels.Scheduled != null)//if parcel is assigned a drones
                    {
                        if (indexOfParcels.PickedUp != null)//if parcel is picked up by drone
                        {
                            if (indexOfParcels.Delivered != null)//parcel is delivered
                                parcelAtCustomer.StateOfParcel = ParcelState.Provided;
                            else
                                parcelAtCustomer.StateOfParcel = ParcelState.PickedUp;
                        }
                        else
                            parcelAtCustomer.StateOfParcel = ParcelState.Paired;
                    }
                    else
                        parcelAtCustomer.StateOfParcel = ParcelState.Created;
                    parcelAtCustomer.SourceOrDestination = new CustomerInParcel();
                    parcelAtCustomer.SourceOrDestination.Id = blCustomer.Id;//Updates the source information of the parcel
                    parcelAtCustomer.SourceOrDestination.Name = blCustomer.Name;//Updates the source information of the parcel
                }
                return parcelAtCustomer;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelAtCustomer> parcelAtCustomers(bool flag, Customer blCustomer, List<DO.Parcel> parcelList)
        {
            lock (dal)
            {
                return from item in parcelList
                       where flag ? item.SenderId == blCustomer.Id : item.TargetId == blCustomer.Id
                       select NewMethod(item, blCustomer);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer GetCustomer(int customerId)
        {
            Customer blCustomer = new Customer();
            lock (dal)
            {
                try
                {
                    DO.Customer dalCustomer = dal.FindCustomer(customerId);//finding customer using inputted id
                    dalCustomer.CopyPropertiesTo(blCustomer);//converting dal->bl
                    blCustomer.CustomerLocation = CopyLocation(dalCustomer.Longitude, dalCustomer.Latitude);
                    blCustomer.ParcelsFromCustomers = new List<ParcelAtCustomer>();
                    blCustomer.ParcelsToCustomers = new List<ParcelAtCustomer>();
                    //goes through the parcels with the sent condition
                    List<DO.Parcel> parcelList = dal.GetAllParcels(parcel => parcel.SenderId == customerId || parcel.TargetId == customerId).ToList();
                    blCustomer.ParcelsFromCustomers = parcelAtCustomers(true, blCustomer, parcelList);
                    blCustomer.ParcelsToCustomers = parcelAtCustomers(false, blCustomer, parcelList);
                }
                catch (DO.ItemDoesNotExistException ex)
                {
                    throw new FailedGetException("ERROR\n", ex);
                }
                return blCustomer;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<CustomerToList> GetAllCustomers(Predicate<CustomerToList> predicate = null)
        {
            lock (dal)
            {
                return (from itemCustomer in dal.GetAllCustomers()
                        let tempCustomer = GetCustomer(itemCustomer.Id)//brings the customer by the ID number
                        select new CustomerToList
                        {
                            Id = tempCustomer.Id,
                            Name = tempCustomer.Name,
                            Phone = tempCustomer.Phone,
                            ParcelsSentAndDelivered = dal.GetAllParcels(item => item.SenderId == tempCustomer.Id && item.Delivered != null).Count(),
                            ParcelsSentButNotDelivered = dal.GetAllParcels(item => item.SenderId == tempCustomer.Id && item.Delivered == null).Count(),
                            RecievedParcels = dal.GetAllParcels(item => item.TargetId == tempCustomer.Id && item.Delivered != null).Count(),
                            ParcelsOnTheWayToCustomer = dal.GetAllParcels(item => item.TargetId == tempCustomer.Id && item.Delivered == null).Count(),
                        }).Where(item => predicate == null ? true : predicate(item));
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomer(int idCustomer, string newName, string customerPhone)
        {
            lock (dal)
            {
                try
                {
                    dal.UpdateCustomer(idCustomer, newName, customerPhone);//sends to update in dal
                }
                catch (DO.ItemDoesNotExistException ex)
                {
                    throw new FailedToAddException("ERROR\n", ex);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteCustomer(int idCustomer)
        {
            lock (dal)
            {
                try
                {
                    dal.DeleteCustomer(idCustomer);//delete customer
                }
                catch (DO.ItemDoesNotExistException ex)
                {
                    throw new ItemDoesNotExistException("ERROR.\n", ex);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool IsActive(int id)
        {
            if (dal.IsActive(id))
                return true;
            return false;
        }

    }
}
