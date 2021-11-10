using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class CustomerInParcel//לקוח בחבילה
        {
            public int Id { get; set; }
            public int Name { get; set; }
            public override string ToString()
            {
                String result = "";
                result += $"Id is {Id} \n";
                result += $"Name is {Name} \n";
                return result;
            }
        }
    }
}
