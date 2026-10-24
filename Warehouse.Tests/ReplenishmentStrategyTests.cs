using System;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Strategy;
using Xunit;

namespace Warehouse.Tests
{
    public class ReplenishmentStrategyTests
    {
        #region FixedReorderPointStrategy Tests

        [Theory]
        [InlineData(5, 10, true)]
        [InlineData(10, 10, true)]
        [InlineData(11, 10, false)]
        public void FixedReorderPoint_RequiresReorder_EvaluatesQuantityAgainstReorderPoint(int quantity, int reorderPoint, bool expectedResult)
        {
            var item = new StockItem("SKU-01", "KHO-A", initialQuantity: quantity, reorderPoint: reorderPoint, targetStockLevel: 25);
            var strategy = new FixedReorderPointStrategy();


            bool result = strategy.RequiresReorder(item, DateTime.Now);


            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void FixedReorderPoint_CalculateReorderQuantity_ReturnsDifferenceTargetMinusCurrent()
        {
            var item = new StockItem("SKU-01", "KHO-A", initialQuantity: 6, reorderPoint: 10, targetStockLevel: 25);
            var strategy = new FixedReorderPointStrategy();


            int reorderQty = strategy.CalculateReorderQuantity(item);


            Assert.Equal(19, reorderQty);
        }

        [Fact]
        public void FixedReorderPoint_RequiresReorder_NullItem_ThrowsArgumentNullException()
        {

            var strategy = new FixedReorderPointStrategy();


            Assert.Throws<ArgumentNullException>(() => strategy.RequiresReorder(null!, DateTime.Now));
        }

        [Fact]
        public void FixedReorderPoint_CalculateReorderQuantity_NullItem_ThrowsArgumentNullException()
        {

            var strategy = new FixedReorderPointStrategy();


            Assert.Throws<ArgumentNullException>(() => strategy.CalculateReorderQuantity(null!));
        }

        #endregion

        #region PeriodicReviewStrategy Tests

        [Fact]
        public void PeriodicReview_OnReviewDayAndQuantityBelowTarget_ReturnsTrue()
        {

            var item = new StockItem("SKU-01", "KHO-A", initialQuantity: 8, reorderPoint: 10, targetStockLevel: 25);
            var strategy = new PeriodicReviewStrategy(DayOfWeek.Monday);
            var mondayDate = new DateTime(2026, 10, 12); // Thứ Hai


            bool result = strategy.RequiresReorder(item, mondayDate);


            Assert.True(result);
        }

        [Fact]
        public void PeriodicReview_NotOnReviewDay_ReturnsFalse()
        {
    
            var item = new StockItem("SKU-01", "KHO-A", initialQuantity: 2, reorderPoint: 10, targetStockLevel: 25);
            var strategy = new PeriodicReviewStrategy(DayOfWeek.Monday);
            var tuesdayDate = new DateTime(2026, 10, 13); // Thứ Ba


            bool result = strategy.RequiresReorder(item, tuesdayDate);


            Assert.False(result);
        }

        [Fact]
        public void PeriodicReview_OnReviewDayButQuantityAtOrAboveTarget_ReturnsFalse()
        {

            var item = new StockItem("SKU-01", "KHO-A", initialQuantity: 25, reorderPoint: 10, targetStockLevel: 25);
            var strategy = new PeriodicReviewStrategy(DayOfWeek.Monday);
            var mondayDate = new DateTime(2026, 10, 12);


            bool result = strategy.RequiresReorder(item, mondayDate);


            Assert.False(result);
        }

        [Fact]
        public void PeriodicReview_CalculateReorderQuantity_ReturnsDifferenceTargetMinusCurrent()
        {

            var item = new StockItem("SKU-01", "KHO-A", initialQuantity: 10, reorderPoint: 10, targetStockLevel: 25);
            var strategy = new PeriodicReviewStrategy(DayOfWeek.Monday);


            int reorderQty = strategy.CalculateReorderQuantity(item);


            Assert.Equal(15, reorderQty);
        }

        [Fact]
        public void PeriodicReview_RequiresReorder_NullItem_ThrowsArgumentNullException()
        {

            var strategy = new PeriodicReviewStrategy(DayOfWeek.Monday);


            Assert.Throws<ArgumentNullException>(() => strategy.RequiresReorder(null!, DateTime.Now));
        }

        [Fact]
        public void PeriodicReview_CalculateReorderQuantity_NullItem_ThrowsArgumentNullException()
        {

            var strategy = new PeriodicReviewStrategy(DayOfWeek.Monday);

            Assert.Throws<ArgumentNullException>(() => strategy.CalculateReorderQuantity(null!));
        }

        #endregion
    }
}