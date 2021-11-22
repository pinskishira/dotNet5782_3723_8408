using System;
using IDAL;
using System.Collections;
using System.Collections.Generic;
using IBL.BO;
using IBL;
using static IBL.BO.Enum;
using DalObject;
using BL.IBL.BO;
using System.Linq;
namespace BL
{
    public partial class BL
    {
        /// <summary>
        /// Performing logical tests on the recieved customer and coverting the customer fields in the dalObject
        /// to the customer fields in the BL.
        /// </summary>
        /// <param name="newCustomer">The new customer</param>
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
                //converting BL customer to dal
                IDAL.DO.Customer tempCustomer = new();
                newCustomer.CopyPropertiesTo(tempCustomer);
                dal.AddCustomer(tempCustomer);//adding to customer array in dal
            }
            catch (IDAL.DO.ItemExistsException ex)
            {
                throw new FailedToAddException("The customer already exists.\n", ex);
            }
        }

        /// <summary>
        /// Displays a specific customer, by converting dcustomer to BL an filling the missing fields.
        /// </summary>
        /// <param name="customerId">Id of customer</param>
        /// <returns>Customer</returns>
        public Customer DisplayCustomer(int customerId)//תצוגת לקוח
        {
            Customer blCustomer = new();
            try
            {
                IDAL.DO.Customer dalCustomer = dal.FindCustomer(customerId);//finding customer using inputted id
                dalCustomer.CopyPropertiesTo(blCustomer);//converting dal->bl
                foreach (var indexOfParcels in dal.GetAllParcels())//goes through list of parcels
                {
                    //If the customer we want is either the sender or the recipient of the package
                    if (indexOfParcels.SenderId == blCustomer.Id || indexOfParcels.TargetId == blCustomer.Id)
                    {
                        ParcelAtCustomer parcelAtCustomer = new();
                        indexOfParcels.CopyPropertiesTo(parcelAtCustomer);// converting dal->bl
                        if (indexOfParcels.Scheduled != DateTime.MinValue)//if parcel is assigned a drones
                        {
                            if (indexOfParcels.PickedUp != DateTime.MinValue)//if parcel is picked up by drone
                            {
                                if (indexOfParcels.Delivered != DateTime.MinValue)//parcel is delivered
                                    parcelAtCustomer.StateOfParcel = (ParcelState)4;
                                else
                                    parcelAtCustomer.StateOfParcel = (ParcelState)3;
                            }
                            else
                                parcelAtCustomer.StateOfParcel = (ParcelState)2;
                        }
                        else
                            parcelAtCustomer.StateOfParcel = (ParcelState)1;
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
                throw new FailedDisplayException("The Id does not exist.\n", ex);
            }
            return blCustomer;
        }

        /// <summary>
        /// Converting BL list to dal and updating the parcel state, and amount of packages sent and delivered
        /// to customer ,then adding to custonerToList.
        /// </summary>
        /// <returns>List of customers</returns>
        public IEnumerable<CustomerToList> ListViewCustomers()
        {
            Customer tempCustomer = new();
            CustomerToList tempCustomerToList = new();
            List<CustomerToList> customerToList = new();
            foreach (var indexCustomer in dal.GetAllCustomers())//goes through list of customers
            {
                tempCustomer = DisplayCustomer(indexCustomer.Id);//brings the customer by the ID number
                tempCustomer.CopyPropertiesTo(tempCustomerToList);//converting dal->bll
                foreach (var parcelsFromCustomers in tempCustomer.ParcelsFromCustomers)//goes over the list of parcels from the customer
                {
                    if (parcelsFromCustomers.StateOfParcel == (ParcelState)4)//if parcel is delivered
                        tempCustomerToList.ParcelsSentAndDelivered++;
                    else//if parcel was not delivered
                        tempCustomerToList.ParcelsSentButNotDelivered++;
                }
                foreach (var parcelsToCustomers in tempCustomer.ParcelsToCustomers)//goes over the list of parcels to the customer
                {
                    if (parcelsToCustomers.StateOfParcel == (ParcelState)4)//if he recieved the parcel
                        tempCustomerToList.RecievedParcels++;
                    else//if the parcel is on the way
                        tempCustomerToList.ParcelsOnTheWayToCustomer++;
                }
                customerToList.Add(tempCustomerToList);
                tempCustomerToList = new();
            }
            return customerToList;
        }

        /// <summary>
        /// Finds customer and sends to update in dal.
        /// </summary>
        /// <param name="idCustomer">Id of customer</param>
        /// <param name="newName">New name of customer</param>
        /// <param name="customerPhone">New phone of customer</param>
        public void UpdateCustomer(int idCustomer, string newName, string customerPhone)
        {
            dal.GetAllCustomers().First(item => item.Id == idCustomer);//finds customer
            dal.UpdateCustomer(idCustomer, newName, customerPhone);//sends ton update in dal
        }

    }
}
