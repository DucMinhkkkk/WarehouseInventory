using System.Dynamic;
using WarehouseIventory.entities;
namespace WarehouseIventory.Strategy;

public class PeriodicReviewStrategy : IReplenishmentStrategy
{
    private readonly DayOfWeek _reviewDay;
    public PeriodicReviewStrategy(DayOfWeek reviewDay = DayOfWeek.Monday)
    {
        _reviewDay = reviewDay;
    }
    public string StrategyName => $"Periodic Review (Đánh giá định kỳ vào {TranslateDayOfWeek(_reviewDay)})";
    public bool RequiresReorder(StockItem item,DateTime currentDate)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));
        if(currentDate.DayOfWeek != _reviewDay) return false;
        return item.Quantity < item.TargetStockLevel;
    }

    public int CalculateReorderQuantity(StockItem item)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));

        int quantityToOrder = item.TargetStockLevel - item.Quantity;
        return Math.Max(0,quantityToOrder);
    }
    private static string TranslateDayOfWeek(DayOfWeek day) => day switch
    {
        DayOfWeek.Monday => "Thứ Hai",
        DayOfWeek.Tuesday => "Thứ Ba",
        DayOfWeek.Wednesday => "Thứ Tư",
        DayOfWeek.Thursday => "Thứ Năm",
        DayOfWeek.Friday => "Thứ Sáu",
        DayOfWeek.Saturday => "Thứ Bảy",
        DayOfWeek.Sunday => "Chủ Nhật",
        _ => day.ToString()
    };
}
