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
        class Parcel
        {
            public int Id { get; set; }
            public CustomerInParcel SenderId { get; set; }
            public CustomerInParcel TargetId { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public DateTime Requested { get; set; }
            public int DroneId { get; set; }
            public DateTime CreatedParcel { get; set; }
            public DateTime Scheduled { get; set; }
            public DateTime Delivered { get; set; }
            public DateTime PickedUp { get; set; }
            public override string ToString()
            {
                String result = "";
                result += $"Id is {Id} \n";
                result += $"SenderId is {SenderId} \n";
                result += $"TargetId is {TargetId} \n";
                result += $"Weight is {Weight} \n";
                result += $"Priority is {Priority} \n";
                result += $"Requested is {Requested} \n";
                result += $"DroneId is {DroneId} \n";
                result += $"CreatedParcel is {CreatedParcel} \n";
                result += $"Scheduled is {Scheduled} \n";
                result += $"Delivered is {Delivered} \n";
                result += $"PickedUp is {PickedUp} \n";
                return result;
            }
        }
    }
}
