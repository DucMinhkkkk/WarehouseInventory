using System;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Exceptions;

namespace Warehouse.Infrastructure.Services
{
    public class TransferService
    {
        private readonly AuditLogger _auditLogger;

        public TransferService(AuditLogger auditLogger)
        {
            _auditLogger = auditLogger;
        }

        public void TransferStock(StockItem source, StockItem destination, int quantity, string userId)
        {
            if (source == null || destination == null)
            {
                throw new InvalidStockOperationException("Source and destination cannot be null.");
            }

            if (source.Sku != destination.Sku)
            {
                throw new InvalidStockOperationException("Cannot transfer stock between different SKUs.");
            }

            source.TransferStock(quantity);

            try
            {
                destination.AddStock(quantity);
            }
            catch (Exception ex)
            {
                source.AddStock(quantity);
                throw new InvalidStockOperationException("Transfer failed during destination update. Rolled back successfully.", ex);
            }

            _auditLogger.LogTransaction(source.Sku, "TRANSFER", quantity, userId);
        }
    }
}