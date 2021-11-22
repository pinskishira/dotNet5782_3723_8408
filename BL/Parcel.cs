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
            public CustomerInParcel SenderId { get; set; }
            public CustomerInParcel TargetId { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public DroneInParcel DroneId { get; set; }
            public DateTime Requested { get; set; }//זמן יצירת חבילה
            public DateTime Scheduled { get; set; }//זמן שיוך חבילה
            public DateTime PickedUp { get; set; }//זמן איסוף
            public DateTime Delivered { get; set; }//זמן אספקה
            public override string ToString()
            {
                String result = "";
                result += $"Id is {Id} \n";
                result += $"SenderId is {SenderId} \n";
                result += $"TargetId is {TargetId} \n";
                result += $"Weight is {Weight} \n";
                result += $"Priority is {Priority} \n";
                result += $"DroneParcel is {DroneId} \n";
                result += $"Requested is {Requested} \n";
                result += $"Scheduled is {Scheduled} \n";
                result += $"Delivered is {Delivered} \n";
                result += $"PickedUp is {PickedUp} \n";
                return result;
            }
        }
    }
}
