using System;

namespace Warehouse.Domain.Exceptions
{
    public class InsufficientStockException : Exception
    {
        public string Sku { get; }
        public int AvailableQuantity { get; }
        public int RequestedQuantity { get; }

        public InsufficientStockException(string sku, int available, int requested) 
            : base($"Cannot deduct {requested} units from SKU '{sku}'. Only {available} units available.")
        {
            Sku = sku;
            AvailableQuantity = available;
            RequestedQuantity = requested;
        }

        public InsufficientStockException(string message) : base(message)
        {
            Sku = string.Empty;
            AvailableQuantity = 0;
            RequestedQuantity = 0;
        }
    }
}