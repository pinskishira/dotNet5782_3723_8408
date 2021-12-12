using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static BO.Enum;

namespace BO
{
    public class ParcelToList
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
            result += $"Sender name is {SenderName} \n";
            result += $"Target name is {TargetName} \n";
            result += $"Weight is {Weight} \n";
            result += $"Priority is {Priority} \n";
            result += $"Parcel State is {StateOfParcel} \n";
            return result;
        }
    }
}
