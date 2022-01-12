using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
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
    public class FailedToAddException : Exception//Failed To Add
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
    public class FailedGetException : Exception//Failed To Get
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
    public class DroneMaintananceException : Exception
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
    public class FailedToCollectParcelException : Exception
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
    public class ParcelDeliveryException : Exception
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

    [Serializable]
    public class MissingInfoException : Exception
    {
        public MissingInfoException() : base() { }
        public MissingInfoException(string message) : base(message) { }
        public MissingInfoException(string message, Exception innerException) : base(message, innerException) { }
        public override string ToString()
        {
            return Message;
        }
    }

    [Serializable]
    public class ThreadErrorException : Exception
    {
        public ThreadErrorException() : base() { }
        public ThreadErrorException(string message) : base(message) { }
        public ThreadErrorException(string message, Exception innerException) : base(message, innerException) { }
        public override string ToString()
        {
            return Message;
        }
    }

    [Serializable]
    public class WrongStatusException : Exception
    {
        public WrongStatusException() : base() { }
        public WrongStatusException(string message) : base(message) { }
        public WrongStatusException(string message, Exception innerException) : base(message, innerException) { }
        public override string ToString()
        {
            return Message;
        }
    }
}                       
