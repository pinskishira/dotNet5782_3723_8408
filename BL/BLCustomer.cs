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
            if ((Math.Round(Math.Floor(Math.Log10(newCustomer.Id))) + 1) != 9)//if name inputted is not 9 digits long
                throw new InvalidInputException("The identification number should be 9 digits long\n");
            if (newCustomer.Name == "\n")
                throw new InvalidInputException("You have to enter a valid name, with letters\n");
            if (newCustomer.Phone.Length != 10)
                throw new InvalidInputException("You have to enter a valid phone, with 10 digits\n");
            if (newCustomer.CustomerLocation.Longitude < -180 || newCustomer.CustomerLocation.Longitude > 180)
                throw new InvalidInputException("The longitude is not valid, enter a longitude point between -180 and 1800\n");
            if (newCustomer.CustomerLocation.Latitude < -90 || newCustomer.CustomerLocation.Latitude > 90)
                throw new InvalidInputException("The Latitude is not valid, enter a Latitude point between -90 and 90\n");
            try
            {
                IDAL.DO.Customer tempCustomer = new();
                newCustomer.CopyPropertiesTo(tempCustomer);
                dal.AddCustomer(tempCustomer);
            }
            catch (IDAL.DO.ItemExistsException ex)
            {
                throw new FailedToAddException("The customer already exists.\n", ex);
            }
        }

        public Customer DisplayCustomer(int customerId)//תצוגת לקוח
        {
            Customer blCustomer = new();
            try
            {
                IDAL.DO.Customer dalCustomer = dal.FindCustomer(customerId);// מחפש את הלקוח לפי מספר מזהה
                dalCustomer.CopyPropertiesTo(blCustomer);//המרה-dal->bl
                foreach (var indexOfParcels in dal.GetAllParcels())//עובר על כל הרשימה של החבילות
                {
                    if (indexOfParcels.SenderId == blCustomer.Id || indexOfParcels.TargetId == blCustomer.Id)//אם הלקוח שאנחנו רוצים הוא או המוסר או המקבל את החבילה
                    {
                        ParcelAtCustomer parcelAtCustomer = new();
                        indexOfParcels.CopyPropertiesTo(parcelAtCustomer);// ממיר את החבילה// dal->bl
                        if (indexOfParcels.Scheduled != DateTime.MinValue)//החבילה שויכה
                        {
                            if (indexOfParcels.PickedUp != DateTime.MinValue)// אספו את החבילה
                            {
                                if (indexOfParcels.Delivered != DateTime.MinValue)//ההזמנה סופקה
                                    parcelAtCustomer.StateOfParcel = (ParcelState)4;
                                else
                                    parcelAtCustomer.StateOfParcel = (ParcelState)3;
                            }
                            else
                                parcelAtCustomer.StateOfParcel = (ParcelState)2;
                        }
                        else
                            parcelAtCustomer.StateOfParcel = (ParcelState)1;
                        parcelAtCustomer.SourceOrDestination.Id = blCustomer.Id;//מעדכן את פרטי מקור של החבילה
                        parcelAtCustomer.SourceOrDestination.Name = blCustomer.Name;//מעדכן את פרטי מקור של החבילה
                        if (indexOfParcels.SenderId == blCustomer.Id)//אם הלקוח שולח את החבילה
                            blCustomer.ParcelsFromCustomers.Add(parcelAtCustomer);
                        else//אם הלקוח מקבל את החבילה
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

        public IEnumerable<CustomerToList> ListViewCustomers()
        {
            Customer tempCustomer = new();
            CustomerToList tempCustomerToList = new();
            List<CustomerToList> customerToList = new();
            foreach (var indexCustomer in dal.GetAllCustomers())
            {
                tempCustomer = DisplayCustomer(indexCustomer.Id);//מביא את הלקוח לפי המספר המזהה
                tempCustomer.CopyPropertiesTo(tempCustomerToList);//המרה//dal->bll
                foreach (var parcelsFromCustomers in tempCustomer.ParcelsFromCustomers)//עובר על הרשימה של החבילות מהלקוח
                {
                    if (parcelsFromCustomers.StateOfParcel == (ParcelState)4)//אם החבילה סופקה
                        tempCustomerToList.ParcelsSentAndDelivered++;
                    else//אם החבילה לא סופקה
                        tempCustomerToList.ParcelsSentButNotDelivered++;
                }
                foreach (var parcelsToCustomers in tempCustomer.ParcelsToCustomers)//עובר על הרשימה של החבילות ללקוח
                {
                    if (parcelsToCustomers.StateOfParcel == (ParcelState)4)//אם הוא קיבל את החבילה
                        tempCustomerToList.RecievedParcels++;
                    else//אם החבילה בדרך
                        tempCustomerToList.ParcelsOnTheWayToCustomer++;
                }
                customerToList.Add(tempCustomerToList);
            }
            return customerToList;
        }

        public void UpdateCustomer(int idCustomer, string newName, string customerPhone)
        {
            dal.GetAllCustomers().First(item => item.Id == idCustomer);
            dal.UpdateCustomer(idCustomer, newName, customerPhone);
        }

    }
}
