using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static IBL.BO.Enum;

namespace IBL
{
    namespace BO
    {
        public class ParcelAtCustomer
        {
            public int Id { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public ParcelState StateOfParcel { get; set; }
            public CustomerInParcel SourceOrDestination { get; set; }
            public override string ToString()//Override function
            {
                String result = "";
                result += $"ID is {Id} \n";
                result += $"Weight is {Weight} \n";
                result += $"Priority is {Priority} \n";
                result += $"State Of Parcel is {StateOfParcel} \n";
                result += $"Source Or Destination is {SourceOrDestination} \n";
                return result;
            }
        }
    }
}
