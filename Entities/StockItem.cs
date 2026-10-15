using System;

namespace Warehouse.Domain.Entities
{
    public class StockItem
    {
        public string StockItemId { get; private set; }
        public Product Product { get; private set; }
        public Location Location { get; private set; }
        
        // Encapsulation: Trạng thái (số lượng) được bảo vệ, không cho phép set tuỳ ý từ bên ngoài
        public int Quantity { get; private set; }

        public StockItem(string stockItemId, Product product, Location location, int initialQuantity = 0)
        {
            if (initialQuantity < 0)
                throw new InvalidOperationException("Số lượng tồn kho ban đầu không thể âm.");

            StockItemId = stockItemId;
            Product = product;
            Location = location;
            Quantity = initialQuantity;
        }

        public void AddStock(int amount)
        {
            if (amount <= 0)
                throw new InvalidOperationException("Số lượng nhập thêm phải lớn hơn 0.");
                
            Quantity += amount;
        }

        public void RemoveStock(int amount)
        {
            if (amount <= 0)
                throw new InvalidOperationException("Số lượng xuất đi phải lớn hơn 0.");
                
            // Guard against negative stock
            if (Quantity - amount < 0)
                throw new InvalidOperationException($"Lỗi: Không đủ hàng trong kho cho sản phẩm {Product.SKU} tại vị trí {Location.Name}.");

            Quantity -= amount;
        }
    }
}