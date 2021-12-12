using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static BO.Enum;

namespace BO
{
    public class ParcelAtCustomer
    {
        public int Id { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public ParcelState StateOfParcel { get; set; }
        public CustomerInParcel SourceOrDestination { get; set; }
        public override string ToString()
        {
            String result = "";
            result += $"ID is {Id} \n";
            result += $"Weight is {Weight} \n";
            result += $"Priority is {Priority} \n";
            result += $"State of parcel is {StateOfParcel} \n";
            result += $"Source or destination is {SourceOrDestination} \n";
            return result;
        }
    }
}
