
using System;
using Warehouse.Domain.Entities;

namespace Warehouse.Domain.Strategy
{
    public class CriticalStockStrategy : IReplenishmentStrategy
    {
        public string StrategyName =>
            "Critical Stock Strategy (50% of ROP)";

        // Chỉ đặt hàng khi tồn kho còn tối đa 50% ngưỡng ROP.
        public bool RequiresReorder(
            StockItem item,
            DateTime currentDate)
        {
            ArgumentNullException.ThrowIfNull(item);

            int criticalThreshold = item.ReorderPoint / 2;

            return item.Quantity <= criticalThreshold;
        }

        // Nhập bù để đạt mức tồn kho mục tiêu.
        public int CalculateReorderQuantity(StockItem item)
        {
            ArgumentNullException.ThrowIfNull(item);

            return Math.Max(
                0,
                item.TargetStockLevel - item.Quantity);
        }
    }
}
