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
            if (DataSource.Customers.Exists(item => item.Id == newCustomer.Id && !newCustomer.DeletedCustomer))//checks if customer exists
                throw new ItemExistsException("The customer already exists.\n");
            DataSource.Customers.Add(newCustomer);
        }

        public Customer FindCustomer(int id)
        {
            int indexCustomer = CheckExistingCustomer(id);//checks if customer exists
            return DataSource.Customers[indexCustomer];//finding parcel
        }

        public IEnumerable<Customer> GetAllCustomers(Predicate<Customer> predicate = null)
        {
            return from itemCustomer in DataSource.Customers
                   where predicate == null ? true : predicate(itemCustomer)
                   where !itemCustomer.DeletedCustomer
                   select itemCustomer;
        }

        public void UpdateCustomer(int idCustomer, string newName, string customerPhone)
        {
            int indexOfCustomer = CheckExistingParcel(idCustomer);//finds if customer exists
            Customer customer = DataSource.Customers[indexOfCustomer];
            if (newName != "")//if enter was entered instead of new name
                customer.Name = newName;
            if (customerPhone != "")//if enter was entered instead of new phone
                customer.Phone = customerPhone;
            DataSource.Customers[indexOfCustomer] = customer;//updated customer into list of customers
        }

        private int CheckExistingCustomer(int id)
        {
            int index = DataSource.Customers.FindIndex(customer => customer.Id == id);
            if (index == -1)
                throw new ItemDoesNotExistException("No customer found with this id");
            if (DataSource.Customers[index].DeletedCustomer)
                throw new ItemDoesNotExistException("This customer is deleted");
            return index;
        }


        public void DeleteCustomer(int id)
        {
            int indexOfCustomer = CheckExistingCustomer(id);//checks if parcel exists
            Customer customer = DataSource.Customers[indexOfCustomer];
            customer.DeletedCustomer = true;
            DataSource.Customers[indexOfCustomer] = customer;
        }
    }
}
