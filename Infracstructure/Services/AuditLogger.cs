using System;
using System.Collections.Generic;
using Warehouse.Domain.Entities;

namespace Warehouse.Infrastructure.Services
{
    public class AuditLogger
    {
        private readonly List<AuditEntry> _auditTrail;

        public AuditLogger()
        {
            _auditTrail = new List<AuditEntry>();
        }

        public void LogTransaction(string sku, string actionType, int quantity, string userId)
        {
            var entry = new AuditEntry(sku, actionType, quantity, userId, DateTime.UtcNow);
            _auditTrail.Add(entry);
            
            Console.WriteLine($"[AUDIT LOG] {entry.Timestamp:yyyy-MM-dd HH:mm:ss} | {entry.UserId} performed {entry.ActionType} of {entry.Quantity} units on SKU: {entry.Sku}");
        }

        public IReadOnlyList<AuditEntry> GetHistory()
        {
            return _auditTrail.AsReadOnly();
        }
    }
}