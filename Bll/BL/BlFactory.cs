using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;

namespace BL
{
    public static class BlFactory
    {
        public static Ibl GetBl()
        {
            return new BL();
        }
    }
}
