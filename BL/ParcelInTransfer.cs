using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IBL.BO.Enum;
using IBL.BO;

namespace BL
{
    class ParcelInTransfer//חבילה בהעברה
    {
        public int Id { get; set; }
        public Priorities Priority { get; set; }
        public CustomerInParcel Sender { get; set; }
        public CustomerInParcel Reciever { get; set; }
    }
}
