using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static BO.Enum;

namespace BO
{
    public class ParcelInTransfer
    {
        public int Id { get; set; }
        public bool ParcelState { get; set; }
        public Priorities Priority { get; set; }
        public WeightCategories Weight { get; set; }
        public CustomerInParcel Sender { get; set; }
        public CustomerInParcel Target { get; set; }
        public Location CollectionLocation { get; set; }
        public Location DeliveryDestination { get; set; }
        public double TransportDistance { get; set; }
        public override string ToString()//Override function
        {
            String result = "";
            result += $"ID is {Id} \n";
            result += $"State Of Parcel is {ParcelState} \n";
            result += $"Priority is {Priority} \n";
            result += $"Weight is {Weight} \n";
            result += $"Sender is {Sender} \n";
            result += $"Target is {Target} \n";
            result += $"Collection location is {CollectionLocation} \n";
            result += $"Delivery destination is {DeliveryDestination} \n";
            result += $"Transport distance is {TransportDistance} \n";
            return result;
        }
    }
}
