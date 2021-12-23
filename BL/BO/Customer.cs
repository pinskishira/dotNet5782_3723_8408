using System;
using System.Collections.Generic;
using System.Linq;

namespace BO
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public Location CustomerLocation { get; set; }
        public IEnumerable<ParcelAtCustomer> ParcelsFromCustomers { get; set; }
        public IEnumerable<ParcelAtCustomer> ParcelsToCustomers { get; set; }
        public override string ToString()
        {
            String result = "";
            result += $"Id is {Id} \n";
            result += $"Name is {Name} \n";
            result += $"Phone is {Phone} \n";
            result += $"Customers' location is {CustomerLocation} \n";
            if (ParcelsFromCustomers !=  null)
            {
                result += $"Parcels from customers is:\n";
                foreach (var indexOfParcelsFromCustomers in ParcelsFromCustomers)
                {
                    result += $"{indexOfParcelsFromCustomers} \n";
                }
            }
            if (ParcelsToCustomers != null)
            {
                result += $"Parcels to customers is: \n";
                foreach (var indexOfParcelsToCustomers in ParcelsToCustomers)
                {
                    result += $"{indexOfParcelsToCustomers} \n";
                }
            }
            return result;
        }
    }
}
