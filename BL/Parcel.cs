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
        public class Parcel
        {
            public int Id { get; set; }
            public CustomerInParcel Sender { get; set; }
            public CustomerInParcel Target { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public DroneInParcel DroneParcel { get; set; }
            public DateTime Requested { get; set; }//זמן יצירת חבילה
            public DateTime Scheduled { get; set; }//זמן שיוך חבילה
            public DateTime PickedUp { get; set; }//זמן איסוף
            public DateTime Delivered { get; set; }//זמן אספקה
            public override string ToString()
            {
                String result = "";
                result += $"Id is {Id} \n";
                result += $"SenderId is \n{Sender}";
                result += $"TargetId is \n{Target}";
                result += $"Weight is {Weight}\n";
                result += $"Priority is {Priority} \n";
                result += $"Drone in parcel is \n{DroneParcel} \n";
                result += $"Requested is {Requested} \n";
                result += $"Scheduled is {Scheduled} \n";
                result += $"Delivered is {Delivered} \n";
                result += $"PickedUp is {PickedUp} \n";
                return result;
            }
        }
    }
}
