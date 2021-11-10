using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IBL.BO.Enum;
using IBL.BO;

namespace BL
{
    class DroneInPackage//רחפן בחבילה
    {
        public int Id { get; set; }
        public int Battery { get; set; }
        public Location CurrentLocation { get; set; }
    }
}
