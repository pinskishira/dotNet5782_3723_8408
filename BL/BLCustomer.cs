using System;
using System.Collections.Generic;
using System.Linq;

using IBL.BO;
using static IBL.BO.Enum;

namespace BL
{
    public partial class BL
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
                IDAL.DO.Customer tempCustomer = new();
                object obj = tempCustomer;
                newCustomer.CopyPropertiesTo(obj);
                tempCustomer = (IDAL.DO.Customer)obj;
                newCustomer.CopyPropertiesTo(tempCustomer);
                dal.AddCustomer(tempCustomer);//adding to station list in dal
            }
            catch (IDAL.DO.ItemExistsException ex)
            {
                throw new FailedToAddException("ERROR.\n", ex);
            }
        }

        public Customer GetCustomer(int customerId)
        {
            Customer blCustomer = new();
            try
            {
                IDAL.DO.Customer dalCustomer = dal.FindCustomer(customerId);//finding customer using inputted id
                dalCustomer.CopyPropertiesTo(blCustomer);//converting dal->bl
                blCustomer.CustomerLocation = CopyLocation(dalCustomer.Longitude, dalCustomer.Latitude);
                blCustomer.ParcelsFromCustomers = new();
                blCustomer.ParcelsToCustomers = new();
                List<IDAL.DO.Parcel> parcels = dal.GetAllParcels().ToList();
                foreach (var indexOfParcels in parcels)//goes through list of parcels
                {
                    ParcelAtCustomer parcelAtCustomer = new();
                    indexOfParcels.CopyPropertiesTo(parcelAtCustomer);// converting dal->bl
                    //If the customer we want is either the sender or the recipient of the package
                    if (indexOfParcels.SenderId == blCustomer.Id || indexOfParcels.TargetId == blCustomer.Id)
                    {
                        if (indexOfParcels.Scheduled != DateTime.MinValue)//if parcel is assigned a drones
                        {
                            if (indexOfParcels.PickedUp != DateTime.MinValue)//if parcel is picked up by drone
                            {
                                if (indexOfParcels.Delivered != DateTime.MinValue)//parcel is delivered
                                    parcelAtCustomer.StateOfParcel = ParcelState.Provided;
                                else
                                    parcelAtCustomer.StateOfParcel = ParcelState.PickedUp;
                            }
                            else
                                parcelAtCustomer.StateOfParcel = ParcelState.Paired;
                        }
                        else
                            parcelAtCustomer.StateOfParcel = ParcelState.Created;
                        parcelAtCustomer.SourceOrDestination = new();
                        parcelAtCustomer.SourceOrDestination.Id = blCustomer.Id;//Updates the source information of the parcel
                        parcelAtCustomer.SourceOrDestination.Name = blCustomer.Name;//Updates the source information of the parcel
                        if (indexOfParcels.SenderId == blCustomer.Id)//If the customer sends the parcel
                            blCustomer.ParcelsFromCustomers.Add(parcelAtCustomer);
                        else//If the customer receives the parcel
                            blCustomer.ParcelsToCustomers.Add(parcelAtCustomer);

                    }
                }
            }
            catch (IDAL.DO.ItemDoesNotExistException ex)
            {
                throw new FailedDisplayException("ERROR\n", ex);
            }
            return blCustomer;
        }

        public IEnumerable<CustomerToList> GetAllCustomers()
        {
            Customer tempCustomer = new();
            CustomerToList tempCustomerToList = new();
            List<CustomerToList> customerToList = new();
            foreach (var indexCustomer in dal.GetAllCustomers())//goes through list of customers
            {
                tempCustomer = GetCustomer(indexCustomer.Id);//brings the customer by the ID number
                tempCustomer.CopyPropertiesTo(tempCustomerToList);//converting dal->bll
                foreach (var parcelsFromCustomers in tempCustomer.ParcelsFromCustomers)//goes over the list of parcels from the customer
                {
                    if (parcelsFromCustomers.StateOfParcel == ParcelState.Provided)//if parcel is delivered
                        tempCustomerToList.ParcelsSentAndDelivered++;
                    else//if parcel was not delivered
                        tempCustomerToList.ParcelsSentButNotDelivered++;
                }
                foreach (var parcelsToCustomers in tempCustomer.ParcelsToCustomers)//goes over the list of parcels to the customer
                {
                    if (parcelsToCustomers.StateOfParcel == ParcelState.Provided)//if he recieved the parcel
                        tempCustomerToList.RecievedParcels++;
                    else//if the parcel is on the way
                        tempCustomerToList.ParcelsOnTheWayToCustomer++;
                }
                customerToList.Add(tempCustomerToList);
                tempCustomerToList = new();
            }
            return customerToList;
        }

        public void UpdateCustomer(int idCustomer, string newName, string customerPhone)
        {
            try
            {
                dal.UpdateCustomer(idCustomer, newName, customerPhone);//sends to update in dal
            }
            catch (IDAL.DO.ItemDoesNotExistException ex)
            {
                throw new FailedToAddException("ERROR\n", ex);
            }
        }

    }
}
