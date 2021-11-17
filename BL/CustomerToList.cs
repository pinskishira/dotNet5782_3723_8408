using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class CustomerToList//לקוח לרשימה
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public long Phone { get; set; }
            public int ParcelsSentAndDelivered { get; set; }
            public int ParcelsSentButNotDelivered { get; set; }
            public int RecievedParcels { get; set; }
            public int ParcelsOnTheWayToCustomer { get; set; }
            public override string ToString()
            {
                String result = "";
                result += $"Id is {Id} \n";
                result += $"Name is {Name} \n";
                result += $"Phone is {Phone} \n";
                result += $"ParcelsSentAndDelivered is {ParcelsSentAndDelivered} \n";
                result += $"ParcelsSentButNotDelivered is {ParcelsSentButNotDelivered} \n";
                result += $"RecivedParcels is {RecievedParcels} \n";
                result += $"ParcelsOnTheWayToCustomer is {ParcelsOnTheWayToCustomer} \n";
                return result;
            }
        }
    }
}
