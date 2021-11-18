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
            public string Phone { get; set; }
            public int ParcelsSentAndDelivered { get; set; }//מספר חבילות ששלח וסופקו
            public int ParcelsSentButNotDelivered { get; set; }//מספר חבילות ששלח אך עוד לא סופקו
            public int RecievedParcels { get; set; }//מספר חבילות שקיבל
            public int ParcelsOnTheWayToCustomer { get; set; }//מספר חבילות שבדרך אל הלקוח
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
