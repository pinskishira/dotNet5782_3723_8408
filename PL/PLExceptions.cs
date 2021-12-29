using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public class PLExceptions
    {
        [Serializable]
        public class UnmatchedDroneStatusException : Exception
        {
            public UnmatchedDroneStatusException() : base() { }
            public UnmatchedDroneStatusException(string message) : base(message) { }
            public UnmatchedDroneStatusException(string message, Exception innerException) : base(message, innerException) { }
            public override string ToString()
            {
                return Message;
            }
        }

        [Serializable]
        public class SameFieldDataException : Exception
        {
            public SameFieldDataException() : base() { }
            public SameFieldDataException(string message) : base(message) { }
            public SameFieldDataException(string message, Exception innerException) : base(message, innerException) { }
            public override string ToString()
            {
                return Message;
            }
        }

        [Serializable]
        public class InvalidInputExceptionPL : Exception
        {
            public InvalidInputExceptionPL() : base() { }
            public InvalidInputExceptionPL(string message) : base(message) { }
            public InvalidInputExceptionPL(string message, Exception innerException) : base(message, innerException) { }
            public override string ToString()
            {
                return Message;
            }
        }


    }
}
