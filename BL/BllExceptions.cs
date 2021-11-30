using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    [Serializable]
    public class InvalidInputException : Exception
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
    public class FailedToAddException : Exception//הוספה נכשלה
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
    public class FailedGetException : Exception//יצוג ישות נכשל
    {
        public FailedGetException() : base() { }
        public FailedGetException(string message) : base(message) { }
        public FailedGetException(string message, Exception innerException) : base(message, innerException) { }
        public override string ToString()
        {
            return Message;
        }
    }

    [Serializable]
    public class DroneMaintananceException : Exception//בעיה בלשלוח רחפן לטעינה
    {
        public DroneMaintananceException() : base() { }
        public DroneMaintananceException(string message) : base(message) { }
        public DroneMaintananceException(string message, Exception innerException) : base(message, innerException) { }
        public override string ToString()
        {
            return Message;
        }
    }

    [Serializable]
    public class FailedToCollectParcelException : Exception//בעיה באיסוף חבילה
    {
        public FailedToCollectParcelException() : base() { }
        public FailedToCollectParcelException(string message) : base(message) { }
        public FailedToCollectParcelException(string message, Exception innerException) : base(message, innerException) { }
        public override string ToString()
        {
            return Message;
        }
    }

    [Serializable]
    public class ParcelDeliveryException : Exception//בעיה בשחרור רחפן מטעינה
    {
        public ParcelDeliveryException() : base() { }
        public ParcelDeliveryException(string message) : base(message) { }
        public ParcelDeliveryException(string message, Exception innerException) : base(message, innerException) { }
        public override string ToString()
        {
            return Message;
        }
    }
    public class ItemDoesNotExistException : Exception
    {
        public ItemDoesNotExistException() : base() { }
        public ItemDoesNotExistException(string message) : base(message) { }
        public ItemDoesNotExistException(string message, Exception innerException) : base(message, innerException) { }
        public override string ToString()
        {
            return Message;
        }
    }
}                       
