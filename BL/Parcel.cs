using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IBL.BO.Enum;

namespace BL
{
    class Parcel
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int TargetId { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public DateTime Requested { get; set; }
        public int DroneId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Scheduled { get; set; }
        public DateTime Delivered { get; set; }
        public DateTime PickedUp { get; set; }
    }
}
