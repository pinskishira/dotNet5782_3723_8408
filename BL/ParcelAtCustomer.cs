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
        public class ParcelAtCustomer//חבילה אצל לקוח
        {
            public int Id { get; set; }
            public WeightCategories Weight { get; set; }
            public DroneStatuses Status { get; set; }
            public State State { get; set; }
            public CustomerInDelivery SourceOrDestination { get; set; }
            public override string ToString()//Override function
            {
                String result = "";
                result += $"ID is {Id} \n";
                result += $"Weight is {Weight} \n";
                result += $"Status is {Status} \n";
                result += $"Target Id is {SourceOrDestination} \n";
                result += $"State is {State} \n";
                return result;
            }
        }
    }
}
