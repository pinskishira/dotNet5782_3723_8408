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
        public class PackageAtCustomer//משלוח אצל לקוח
        {
            public int Id { get; set; }
            public WeightCategories Weight { get; set; }
            public DroneStatuses Status { get; set; }
            public State State { get; set; }
            public CustomerInParcel SourceOrDestination { get; set; }
        }
    }
}
