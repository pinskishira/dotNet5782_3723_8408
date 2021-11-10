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
        public class PackageInTranser//משלוח בהעברה
        {
            public int Id { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public DroneStatuses Status { get; set; }
            public Location CollectionLocation { get; set; }
            public Location DeliveryDestination { get; set; }
            public double TransportDistance { get; set; }
        }
    }
}
