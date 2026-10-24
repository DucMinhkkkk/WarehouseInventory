using System;

namespace Warehouse.Domain.Exceptions
{
    public class InvalidStockOperationException : Exception
    {
        public InvalidStockOperationException(string message) 
            : base(message) { }

        public InvalidStockOperationException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}