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
        public class ItemExistsInList : Exception
        {
            public ItemExistsInList() : base() { }
            public ItemExistsInList(string message) : base(message) { }
            public ItemExistsInList(string message, Exception innerException) : base(message, innerException) { }
            public override string ToString()
            {
                return Message;
            }
        }
        [Serializable]
        public class ItemDoesNotExistInList : Exception
        {
            public ItemDoesNotExistInList() : base() { }
            public ItemDoesNotExistInList(string message) : base(message) { }
            public ItemDoesNotExistInList(string message, Exception innerException) : base(message, innerException) { }
            public override string ToString()
            {
                return Message;
            }
        }

    }
}
