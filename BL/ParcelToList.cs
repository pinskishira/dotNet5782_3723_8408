using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IBL.BO.Enum;
using IBL.BO;

namespace BL
{
    public class ParcelToList//חבילה לרשימה
    {
        public int Id { get; set; }
        public string SenderName { get; set; }
        public string TargetName { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public ParcelState StateOfParcel { get; set; }
        public override string ToString()//Override function
        {
            String result = "";
            result += $"ID is {Id} \n";
            result += $"Sender Id is {SenderName} \n";
            result += $"Target Id is {TargetName} \n";
            result += $"Weight is {Weight} \n";
            result += $"Priority is {Priority} \n";
            result += $"Parcel State is {StateOfParcel} \n";
            return result;
        }
    }
}
