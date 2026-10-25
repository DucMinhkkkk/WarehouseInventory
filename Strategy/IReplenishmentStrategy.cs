using System;
using Warehouse.Domain.Entities;
namespace Warehouse.Domain.Strategy
{
    public interface IReplenishmentStrategy
    {
        string StrategyName{get;}
        bool RequiresReorder(StockItem item,DateTime currentDate);
        int CalculateReorderQuantity(StockItem item);
    }
}


