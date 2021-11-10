using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    class CustomerToList//לקוח לרשימה
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long Phone { get; set; }
        public int ParcelsSentAndDelivered { get; set; }
        public int ParcelsSentButNotDelivered{ get; set; }
        public int RecivedParcels { get; set; }
        public int ParcelsOnTheWayToCustomer { get; set; }
    }
}
