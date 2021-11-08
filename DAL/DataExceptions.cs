using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    namespace IDAL.DO
    {
        [Serializable]
        public class DataExceptions : Exception
        {
            public DataExceptions() : base() { }
            public DataExceptions(string message) : base(message) { }
            public DataExceptions(string message, Exception inner) : base(message, inner) { }
            public override string ToString()
            {
                return Message;
            }
        }
        //[Serializable]
        //public class ItemExistsInList : Exception
        //{
        //    public ItemExistsInList() : base() { }
        //    public ItemExistsInList(string message) : base(message){ }
        //    public ItemExistsInList(string message, Exception innerException) : base(message, innerException){}
        //    public override string ToString()
        //    {
        //        return Message;
        //    }
        //}

    }
}
