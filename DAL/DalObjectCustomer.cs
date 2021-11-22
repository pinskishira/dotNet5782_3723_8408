﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
//using DAL.IDAL.DO;
using IDAL;

namespace DalObject
{
    public partial class DalObject
    {
        /// <summary>
        /// Adding a new customer to the array of customers
        /// </summary>
        /// <param name="newCustomer">The new customer</param>
        public void AddCustomer(Customer newCustomer)
        {
            if (!DataSource.Customers.Exists(item => item.Id == newCustomer.Id))
                throw new ItemExistsException("The customer already exists.\n");
            DataSource.Customers.Add(newCustomer);
        }
        /// <summary>
        /// Finding requested custmer according to its ID name
        /// </summary>
        /// <param name="id">Wanted customer</param>
        /// <returns></returns>
        public Customer FindCustomer(int id)
        {
            if (!DataSource.Customers.Exists(item => item.Id == id))
                throw new ItemDoesNotExistException("The customer does not exist.\n");
            int indexFindCustomer = DataSource.Customers.FindIndex(indexOfCustomer => indexOfCustomer.Id == id);//Going through customers array
            return DataSource.Customers[indexFindCustomer];
        }
        /// <summary>
        /// Gives a view of the array of customers
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Customer> GetAllCustomers()
        {
            List<Customer> tempCustomers = new();
            foreach (var indexOfCustomers in DataSource.Customers)
            {
                tempCustomers.Add(indexOfCustomers);
            }
            return tempCustomers;
        }

        public void UpdateCustomer(int idCustomer, string newName, string customerPhone)
        {
            int indexOfCustomer = 0;
            Customer customer = new();
            foreach (var indexCustomer in DataSource.Customers)
            {
                if(indexCustomer.Id == idCustomer)
                {
                    customer = indexCustomer;
                    if (newName != "\n")
                        customer.Name = newName;
                    if(customerPhone != "\n")
                        customer.Phone = customerPhone;
                    break;
                }
                indexOfCustomer++;
            }
            DataSource.Customers[indexOfCustomer] = customer;
        }
    }
}
