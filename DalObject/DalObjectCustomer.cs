using System.Collections.Generic;
using System;

using DO;
using System.Linq;

namespace Dal
{
    partial class DalObject
    {
        public void AddCustomer(Customer newCustomer)
        {
            if (DataSource.Customers.Exists(item => item.Id == newCustomer.Id))//checks if customer exists
                throw new ItemExistsException("The customer already exists.\n");
            DataSource.Customers.Add(newCustomer);
        }

        public Customer FindCustomer(int id)
        {
            try
            {
                return DataSource.Customers.First(item => item.Id == id);//checks if customer exists
            }
            catch (InvalidOperationException)
            {
                throw new ItemExistsException("The customer does not exist.\n");
            }
        }

        public IEnumerable<Customer> GetAllCustomers(Predicate<Customer> predicate = null)
        {
            return from itemCustomer in DataSource.Customers
                   where predicate == null ? true : predicate(itemCustomer)
                   select itemCustomer;
        }

        public void UpdateCustomer(int idCustomer, string newName, string customerPhone)
        {
            int indexOfCustomer = DataSource.Customers.FindIndex(item => item.Id == idCustomer);//finds index where customer is
            if (indexOfCustomer == -1)//checks if customer exists
                throw new ItemDoesNotExistException("The customer does not exist.\n");
            Customer customer = DataSource.Customers[indexOfCustomer];
            if (newName != "")//if enter was entered instead of new name
                customer.Name = newName;
            if (customerPhone != "")//if enter was entered instead of new phone
                customer.Phone = customerPhone;
            DataSource.Customers[indexOfCustomer] = customer;//updated customer into list of customers
        }
    }
}
