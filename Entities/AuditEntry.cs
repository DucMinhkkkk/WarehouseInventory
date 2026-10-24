using System;

namespace Warehouse.Domain.Entities
{
    public class AuditEntry
    {
        public Guid AuditId { get; private set; }
        public DateTime Timestamp { get; private set; }
        public string User { get; private set; }
        public string Action { get; private set; }
        public string Details { get; private set; }

        public AuditEntry(string user, string action, string details)
        {
            AuditId = Guid.NewGuid();
            Timestamp = DateTime.UtcNow;
            User = user;
            Action = action;
            Details = details;
        }
    }
}