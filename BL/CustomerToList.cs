﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class CustomerToList
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public int ParcelsSentAndDelivered { get; set; }//Number of parcels sent and delivered
            public int ParcelsSentButNotDelivered { get; set; }//Number of parcels sent but not yet delivered
            public int RecievedParcels { get; set; }//Number of parcels received
            public int ParcelsOnTheWayToCustomer { get; set; }//Amount of parcels on the way to customer
            public override string ToString()
            {
                String result = "";
                result += $"Id is {Id} \n";
                result += $"Name is {Name} \n";
                result += $"Phone is {Phone} \n";
                result += $"Parcels Sent And Delivered is {ParcelsSentAndDelivered} \n";
                result += $"Parcels Sent But Not Delivered is {ParcelsSentButNotDelivered} \n";
                result += $"Recived Parcels is {RecievedParcels} \n";
                result += $"Parcels On The Way To Customer is {ParcelsOnTheWayToCustomer} \n";
                return result;
            }
        }
    }
}
