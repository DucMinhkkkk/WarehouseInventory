using System;

namespace Warehouse.Domain.Entities
{
    public class StockItem
    {
        public string StockItemId { get; private set; }
        public Product Product { get; private set; }
        public Location Location { get; private set; }
        public ReorderPoint {get;set; }
        public TargetStockLevel {get;set;}        
        public int Quantity { get; private set; }

        public StockItem(string stockItemId, Product product, Location location, int initialQuantity = 0,int reorderPoint, 
        int targetStockLevel)
        {
            if (initialQuantity < 0)
                throw new InvalidOperationException("Số lượng tồn kho ban đầu không thể âm.");
            UpdateReorderPolicy(int newReorderPoint,int newTargetStockLevel)

            StockItemId = stockItemId;
            Product = product;
            Location = location;
            Quantity = initialQuantity;
            ReorderPoint = newReorderPoint;
            TargetStockLevel = newTargetStockLevel;
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
            
            if (Quantity - amount < 0)
                throw new InvalidOperationException($"Lỗi: Không đủ hàng trong kho cho sản phẩm {Product.SKU} tại vị trí {Location.Name}.");

            Quantity -= amount;
        }
        public void UpdateReorderPolicy(int newReorderPoint,int newTargetStockLevel){
            if(newReorderPoint < 0){
                throw new InvalidOperationException("Ngưỡng đặt hàng không được âm.");
            }
            if (newTargetStockLevel < newReorderPoint)
                throw new InvalidOperationException("Mức tồn mục tiêu phải lớn hơn hoặc bằng Ngưỡng đặt hàng.");
            ReorderPoint = newReorderPoint;
            TargetStockLevel = newTargetStockLevel;
        }
    }
}
