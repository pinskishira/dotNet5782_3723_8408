using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Parcel
        {
            public int Id { get; set; }
            public int SenderId { get; set; }
            public int TargetId { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public DateTime Requested { get; set; }
            public int Droneld { get; set; }
            public DateTime Scheduled { get; set; }
            public DateTime Delivered { get; set; }
            public DateTime PickedUp { get; set; }
            public override string ToString()//Override function
            {
                String result = " ";
                result += $"ID is {Id}, \n";
                result += $"SenderId is {SenderId}, \n";
                result += $"TargetId is {TargetId}, \n";
                result += $"Weight is {Weight}, \n";
                result += $"Priority is {Priority}, \n";
                result += $"Requested is {Requested}, \n";
                result += $"Droneld is {Droneld}, \n";
                result += $"Scheduled is {Scheduled}, \n";
                result += $"PickedUp is {PickedUp}, \n";
                result += $"Delivered is {Delivered}, \n";
                return result;
            }
        }
    }
}
