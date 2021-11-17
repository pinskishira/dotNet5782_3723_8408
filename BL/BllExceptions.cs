using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BL
{
    namespace IBL.BO
    {
        [Serializable]
        public class InvalidInputException : Exception//קלט לא חוקי
        {
            public InvalidInputException() : base() { }
            public InvalidInputException(string message) : base(message) { }
            public InvalidInputException(string message, Exception innerException) : base(message, innerException) { }
            public override string ToString()
            {
                return Message;
            }
        }
        [Serializable]
        public class FailedToAddException : Exception//קלט לא חוקי
        {
            public FailedToAddException() : base() { }
            public FailedToAddException(string message) : base(message) { }
            public FailedToAddException(string message, Exception innerException) : base(message, innerException) { }
            public override string ToString()
            {
                return Message;
            }
        }
        [Serializable]
        public class FailedDisplayException : Exception//קלט לא חוקי
        {
            public FailedDisplayException() : base() { }
            public FailedDisplayException(string message) : base(message) { }
            public FailedDisplayException(string message, Exception innerException) : base(message, innerException) { }
            public override string ToString()
            {
                return Message;
            }
        }
    }
}
