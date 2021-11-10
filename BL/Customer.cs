using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BL;

namespace IBL
{
    namespace BO
    {
        public class Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public Location CustomerLocation { get; set; }
            public List<Parcel> FromCustomer { get; set; }
            public List<Parcel> ToCustomer { get; set; }
        }
    }
}
