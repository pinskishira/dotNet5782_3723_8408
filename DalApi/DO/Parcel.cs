using System;
using static DO.Enum;

namespace DO
{
    public struct Parcel
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int TargetId { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public int DroneId { get; set; }
        public DateTime? Requested { get; set; }
        public DateTime? Scheduled { get; set; }
        public DateTime? PickedUp { get; set; }
        public DateTime? Delivered { get; set; }
        public bool DeletedParcel { get; set; }
        public override string ToString()
        {
            String result = "";
            result += $"ID is {Id} \n";
            result += $"SenderId is {SenderId} \n";
            result += $"TargetId is {TargetId} \n";
            result += $"Weight is {Weight} \n";
            result += $"Priority is {Priority} \n";
            result += $"Droneld is {DroneId} \n";
            result += $"Requested is {Requested} \n";
            result += $"Scheduled is {Scheduled} \n";
            result += $"PickedUp is {PickedUp} \n";
            result += $"Delivered is {Delivered} \n";
            return result;
        }
    }
}
