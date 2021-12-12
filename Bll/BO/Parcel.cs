using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static BO.Enum;

namespace BO
{
    public class Parcel
    {
        public int Id { get; set; }
        public CustomerInParcel Sender { get; set; }
        public CustomerInParcel Target { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public DroneInParcel DroneParcel { get; set; }
        public DateTime? Requested { get; set; }//Time parcel is created  
        public DateTime? Scheduled { get; set; }//Time parcel is assigned to drone
        public DateTime? PickedUp { get; set; }//Time parcel is picked up by drone
        public DateTime? Delivered { get; set; }//Time parcel is delivered by drone
        public override string ToString()
        {
            String result = "";
            result += $"Id is {Id} \n";
            result += $"SenderId is \n{Sender}";
            result += $"TargetId is \n{Target}";
            result += $"Weight is {Weight}\n";
            result += $"Priority is {Priority} \n";
            result += $"Drone in parcel is: \n{DroneParcel} \n";
            result += $"Requested is {Requested} \n";
            result += $"Scheduled is {Scheduled} \n";
            result += $"Delivered is {Delivered} \n";
            result += $"Picked Up is {PickedUp} \n";
            return result;
        }
    }
}
