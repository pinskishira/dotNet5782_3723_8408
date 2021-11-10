using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    class StationToList//תחתנת בסיס לרשימה
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AvailableChargingSlots { get; set; }
        public int UnavaialbleChargingSlots { get; set; }
    }
}
