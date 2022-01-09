using System;
using System.Collections.Generic;
using System.Linq;

using BO;
using static BO.Enum;

namespace BL
{
    partial class BL
    {
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
            try
            {
                DO.Customer tempCustomer = new DO.Customer();
                object obj = tempCustomer;
                newCustomer.CopyPropertiesTo(obj);
                tempCustomer = (DO.Customer)obj;
                newCustomer.CopyPropertiesTo(tempCustomer);
                dal.AddCustomer(tempCustomer);//adding to station list in dal
            }
            catch (DO.ItemExistsException ex)
            {
                throw new FailedToAddException("ERROR.\n", ex);
            }
        }


        private ParcelAtCustomer NewMethod(DO.Parcel indexOfParcels, Customer blCustomer)
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
        public IEnumerable<ParcelAtCustomer> parcelAtCustomers(bool flag, Customer blCustomer, List<DO.Parcel> parcelList)
        {
            return from item in parcelList
                   where flag ? item.SenderId == blCustomer.Id : item.TargetId == blCustomer.Id
                   select NewMethod(item, blCustomer);
        }

        public Customer GetCustomer(int customerId)
        {
            Customer blCustomer = new Customer();
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

        public IEnumerable<CustomerToList> GetAllCustomers(Predicate<CustomerToList> predicate = null)
        {
            Customer tempCustomer = new Customer();
            CustomerToList tempCustomerToList = new CustomerToList();
            List<CustomerToList> customerToList = new List<CustomerToList>();
            List<DO.Customer> customerList = dal.GetAllCustomers().ToList();
            //customerToList = from item in customerList
            //                 let tempCustomer = GetCustomer(item.Id)

            //                 select new CustomerToList()
            //                 {
            //                     tempCustomer.CopyPropertiesTo(tempCustomerToList)
            //                     from item2 in tempCustomer.ParcelsFromCustomers
            //                     select tempCustomerToList=(item2.StateOfParcel==ParcelState.Provided)?(tempCustomerToList.ParcelsSentAndDelivered++):(tempCustomerToList.ParcelsSentButNotDelivered++)
            //                     from item3 in tempCustomer.ParcelsToCustomers
            //                     select tempCustomerToList=(item3.StateOfParcel==ParcelState.Provided)?(tempCustomerToList.ParcelsSentAndDelivered++):(tempCustomerToList.ParcelsSentButNotDelivered++)

            //                 };


            foreach (var indexCustomer in customerList)//goes through list of customers
            {
                tempCustomer = GetCustomer(indexCustomer.Id);//brings the customer by the ID number
                tempCustomer.CopyPropertiesTo(tempCustomerToList);//converting dal->bll
                IEnumerable<ParcelAtCustomer> ParcelsFromCustomerList = tempCustomer.ParcelsFromCustomers;
                foreach (var parcelsFromCustomers in ParcelsFromCustomerList)//goes over the list of parcels from the customer
                {
                    if (parcelsFromCustomers.StateOfParcel == ParcelState.Provided)//if parcel is delivered
                        tempCustomerToList.ParcelsSentAndDelivered++;
                    else//if parcel was not delivered
                        tempCustomerToList.ParcelsSentButNotDelivered++;
                }
                IEnumerable<ParcelAtCustomer> ParcelsToCustomersList = tempCustomer.ParcelsToCustomers;
                foreach (var parcelsToCustomers in ParcelsToCustomersList)//goes over the list of parcels to the customer
                {
                    if (parcelsToCustomers.StateOfParcel == ParcelState.Provided)//if he recieved the parcel
                        tempCustomerToList.RecievedParcels++;
                    else//if the parcel is on the way
                        tempCustomerToList.ParcelsOnTheWayToCustomer++;
                }
                customerToList.Add(tempCustomerToList);
                tempCustomerToList = new CustomerToList();
            }
            return customerToList.FindAll(item => predicate == null ? true : predicate(item));
        }

        public void UpdateCustomer(int idCustomer, string newName, string customerPhone)
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
}
