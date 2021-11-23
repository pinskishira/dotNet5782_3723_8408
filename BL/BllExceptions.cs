﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public class FailedDisplayException : Exception//יצוג ישות נכשל
    {
        public FailedDisplayException() : base() { }
        public FailedDisplayException(string message) : base(message) { }
        public FailedDisplayException(string message, Exception innerException) : base(message, innerException) { }
        public override string ToString()
        {
            return Message;
        }
    }

    [Serializable]
    public class FailedSendDroneToChargingException : Exception//בעיה בלשלוח רחפן לטעינה
    {
        public FailedSendDroneToChargingException() : base() { }
        public FailedSendDroneToChargingException(string message) : base(message) { }
        public FailedSendDroneToChargingException(string message, Exception innerException) : base(message, innerException) { }
        public override string ToString()
        {
            return Message;
        }
    }

    [Serializable]
    public class FailedToDeliverParcelException : Exception//בעיה בשליחת חבילה
    {
        public FailedToDeliverParcelException() : base() { }
        public FailedToDeliverParcelException(string message) : base(message) { }
        public FailedToDeliverParcelException(string message, Exception innerException) : base(message, innerException) { }
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
    public class FailedReleaseDroneFromChargingException : Exception//בעיה בשחרור רחפן מטעינה
    {
        public FailedReleaseDroneFromChargingException() : base() { }
        public FailedReleaseDroneFromChargingException(string message) : base(message) { }
        public FailedReleaseDroneFromChargingException(string message, Exception innerException) : base(message, innerException) { }
        public override string ToString()
        {
            return Message;
        }
    }

    [Serializable]
    public class FailedAssignParcelToDroneException : Exception//בעיה בשחרור רחפן מטעינה
    {
        public FailedAssignParcelToDroneException() : base() { }
        public FailedAssignParcelToDroneException(string message) : base(message) { }
        public FailedAssignParcelToDroneException(string message, Exception innerException) : base(message, innerException) { }
        public override string ToString()
        {
            return Message;
        }
    }
}                       
