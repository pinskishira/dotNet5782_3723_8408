using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace BlApi
{
    public static class BlFactory
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static Ibl GetBl()
        {
            return BL.BL.Instance;
        }
    }
}
