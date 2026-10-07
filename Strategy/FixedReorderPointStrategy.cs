using WarehouseInventory.Domain.entities;
namespace WarehouseInventory.Strategy;

public class FixedReorderPointStrategy : IReplenishmentStrategy
{
    public string StrategyName => "Fixed Reorder Point (Continuous Review)";

    public bool RequiresReorder(StockItem item, DateTime currentDate)
    {
        ArgumentNullException.ThrowIfNull(item,nameof(item));
        return item.Quantity <= item.ReorderPoint;
    }

    public int CalculateReorderQuantity(StockItem item)
    {
        ArgumentNullException.ThrowIfNull(item,nameof(item));
        int quantityToOrder = item.TargetStockLevel - item.Quantity;
        return Math.Max(0, item.TargetStockLevel - item.Quantity);
    }
}