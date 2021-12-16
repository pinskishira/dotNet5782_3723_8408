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

        //public Customer FindCustomer(int id)
        //{
        //    if (!DataSource.Customers.Exists(item => item.Id == id))//checks if customer exists
        //        throw new ItemDoesNotExistException("The customer does not exist.\n");
        //    return DataSource.Customers[DataSource.Customers.FindIndex(indexOfCustomer => indexOfCustomer.Id == id)];//find customer
        //}
        //public void AddCustomer(Customer newCustomer)
        //{
        //    if (DataSource.Customers.Any(item => item.Id == newCustomer.Id))//checks if customer exists
        //        throw new ItemExistsException("The customer already exists.\n");
        //    DataSource.Customers.Add(newCustomer);
        //}

        public Customer FindCustomer(int id)
        {

            if (!DataSource.Customers.Any(item => item.Id == id))//checks if customer exists
                throw new ItemDoesNotExistException("The customer does not exist.\n");
            Customer customer1 = DataSource.Customers.FirstOrDefault(item => item.Id == id);
            return customer1;



            if (!DataSource.Customers.Any(item => item.Id == id))//checks if customer exists
                throw new ItemDoesNotExistException("The customer does not exist.\n");
            var customer = from item in DataSource.Customers
                           where item.Id == id
                           select item;
            return customer.First();
        }

        public IEnumerable<Customer> GetAllCustomers(Predicate<Customer> predicate = null)
        {
            return DataSource.Customers.FindAll(item => predicate == null ? true : predicate(item));
        }

        public void UpdateCustomer(int idCustomer, string newName, string customerPhone)
        {
            if (!DataSource.Customers.Exists(item => item.Id == idCustomer))//checks if customer exists
                throw new ItemDoesNotExistException("The customer does not exist.\n");
            int indexOfCustomer = DataSource.Customers.FindIndex(item => item.Id == idCustomer);//finds index where customer is
            Customer customer = DataSource.Customers[indexOfCustomer];
            if (newName != "")//if enter was entered instead of new name
                customer.Name = newName;
            if (customerPhone != "")//if enter was entered instead of new phone
                customer.Phone = customerPhone;
            DataSource.Customers[indexOfCustomer] = customer;//updated customer into list of customers
        }
    }
}
