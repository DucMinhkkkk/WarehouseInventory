using WarehouseInventory.Domain.entities;
using WarehouseInventory.Domain.Strategy;

namespace WarehouseInventory.Infrastructure.Reporting;

public class LowStockReportItem
{
    public string Sku { get; set; } = string.Empty;
    public string LocationId { get; set; } = string.Empty;
    public int CurrentQuantity { get; set; }
    public int RecommendedReorderQuantity { get; set; }
    public string StrategyUsed { get; set; } = string.Empty;
}

public class LowStockReportGenerator
{
    public List<LowStockReportItem> GenerateReport(IEnumerable<StockItem> items
    , IReplenishmentStrategy strategy,DateTime currentDate)
    {
        ArgumentNullException.ThrowIfNull(items);
        ArgumentNullException.ThrowIfNull(strategy);

        var report = new List<LowStockReportItem>();
        foreach(var item in items)
        {
            if(strategy.RequiresReorder(item, currentDate))
            {
                report.Add(new LowStockReportItem
                {
                    Sku = item.Sku,
                    LocationId = item.LocationId,
                    CurrentQuantity = item.Quantity,
                    RecommendedReorderQuantity = strategy.CalculateReorderQuantity(item),
                    StrategyUsed = strategy.StrategyName
                });
            }
        }
        return report;
    }
}
