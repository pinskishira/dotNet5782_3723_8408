using System;


namespace DO
{
    [Serializable]
    public class ItemExistsException : Exception
    {
        public ItemExistsException() : base() { }
        public ItemExistsException(string message) : base(message) { }
        public ItemExistsException(string message, Exception innerException) : base(message, innerException) { }
        public override string ToString()
        {
            return Message;
        }
    }
    [Serializable]
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
    public class XMLFileLoadCreateException : Exception
    {
        public string xmlFilePath;
        public XMLFileLoadCreateException(string xmlPath) : base() { xmlFilePath = xmlPath; }
        public XMLFileLoadCreateException(string xmlPath, string message) :
            base(message)
        { xmlFilePath = xmlPath; }
        public XMLFileLoadCreateException(string xmlPath, string message, Exception innerException) :
            base(message, innerException)
        { xmlFilePath = xmlPath; }
        public override string ToString()
        {
            return base.ToString() + $", fail to load or create xml file: {xmlFilePath}";
        }
    }
}

