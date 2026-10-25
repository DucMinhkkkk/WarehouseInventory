using System;

namespace Warehouse.Domain.Entities
{
    public class AuditEntry
    {
        public Guid AuditId { get; private set; }
        public DateTime Timestamp { get; private set; }
        public string UserId { get; private set; }
        public string ActionType { get; private set; }
        public int Quantity { get; private set; }
        public string Sku { get; private set; }
        public string LocationId { get; private set; }
        public string Operator => UserId;
        public string Action => ActionType;
        public int QuantityDelta => Quantity;

        public AuditEntry(string sku,
            string actionType,
            int quantity,
            string userId,
            DateTime timestamp,
            string locationId = "")
        {
            AuditId = Guid.NewGuid();
            Timestamp = timestamp;
            UserId = userId;
            ActionType = actionType;
            Quantity = quantity;
            Sku = sku;
            LocationId = locationId;
        }
    }
}