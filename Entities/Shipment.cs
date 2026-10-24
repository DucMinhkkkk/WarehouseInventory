using System;
using System.Collections.Generic;

namespace Warehouse.Domain.Entities
{
    // Định nghĩa loại Shipment như yêu cầu của file
    public enum ShipmentType
    {
        Inbound,
        Outbound
    }

    public class Shipment
    {
        public string ShipmentId { get; private set; }
        public ShipmentType Type { get; private set; }
        public DateTime ShipmentDate { get; private set; }
        public Location TargetLocation { get; private set; }
        
        // Danh sách các sản phẩm và số lượng trong lô hàng
        public Dictionary<Product, int> Items { get; private set; }

        public Shipment(string shipmentId, ShipmentType type, Location targetLocation)
        {
            ShipmentId = shipmentId;
            Type = type;
            TargetLocation = targetLocation;
            ShipmentDate = DateTime.UtcNow;
            Items = new Dictionary<Product, int>();
        }

        public void AddItem(Product product, int quantity)
        {
            if (quantity <= 0) 
                throw new InvalidOperationException("Số lượng sản phẩm trong lô hàng phải lớn hơn 0.");
                
            if (Items.ContainsKey(product))
                Items[product] += quantity;
            else
                Items.Add(product, quantity);
        }
    }
}