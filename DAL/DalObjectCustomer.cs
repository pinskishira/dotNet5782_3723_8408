using System.Collections.Generic;

using IDAL.DO;

namespace DalObject
{
    public partial class DalObject
    {
        public void AddCustomer(Customer newCustomer)
        {
            if (DataSource.Customers.Exists(item => item.Id == newCustomer.Id))
                throw new ItemExistsException("The customer already exists.\n");
            DataSource.Customers.Add(newCustomer);
        }

        public Customer FindCustomer(int id)
        {
            if (!DataSource.Customers.Exists(item => item.Id == id))
                throw new ItemDoesNotExistException("The customer does not exist.\n");
            int indexFindCustomer = DataSource.Customers.FindIndex(indexOfCustomer => indexOfCustomer.Id == id);//Going through customers array
            return DataSource.Customers[indexFindCustomer];
        }

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
            foreach (var indexCustomer in DataSource.Customers)//going through list of customers
            {
                if(indexCustomer.Id == idCustomer)
                {
                    customer = indexCustomer;
                    if (newName != "\n")//if enter wasnt entered instead of new name
                        customer.Name = newName;
                    if(customerPhone != "\n")//if enter wasnt entered instead of new phone
                        customer.Phone = customerPhone;
                    break;
                }
                indexOfCustomer++;
            }
            DataSource.Customers[indexOfCustomer] = customer;//updated customer into list of customers
        }
    }
}
