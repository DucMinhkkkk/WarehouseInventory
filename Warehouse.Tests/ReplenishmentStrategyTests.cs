using System;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Strategy;
using Xunit;

namespace Warehouse.Tests
{
    public class ReplenishmentStrategyTests
    {
        private readonly Product _product =
            new Product("SKU-01", "Test Product", "");

        private readonly Location _location =
            new Location("KHO-A", "Kho A");

        private StockItem CreateItem(
            int quantity,
            int reorderPoint = 10,
            int targetStockLevel = 25)
        {
            return new StockItem(
                "STOCK-01",
                _product,
                _location,
                initialQuantity: quantity,
                reorderPoint: reorderPoint,
                targetStockLevel: targetStockLevel);
        }

        #region FixedReorderPointStrategy Tests

        [Theory]
        [InlineData(5, 10, true)]
        [InlineData(10, 10, true)]
        [InlineData(11, 10, false)]
        public void FixedReorderPoint_RequiresReorder_EvaluatesQuantityAgainstReorderPoint(int quantity, int reorderPoint, bool expectedResult)
        {
            var item = CreateItem(quantity, reorderPoint: reorderPoint);

            var strategy = new FixedReorderPointStrategy();
            bool result = strategy.RequiresReorder(item, DateTime.Now);


            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void FixedReorderPoint_CalculateReorderQuantity_ReturnsDifferenceTargetMinusCurrent()
        {
            var item = CreateItem(quantity: 6);
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

            var item = CreateItem(quantity: 8);
            var strategy = new PeriodicReviewStrategy(DayOfWeek.Monday);
            var mondayDate = new DateTime(2026, 10, 12); // Thứ Hai


            bool result = strategy.RequiresReorder(item, mondayDate);


            Assert.True(result);
        }

        [Fact]
        public void PeriodicReview_NotOnReviewDay_ReturnsFalse()
        {
    
            var item = CreateItem(quantity: 2);
            var strategy = new PeriodicReviewStrategy(DayOfWeek.Monday);
            var tuesdayDate = new DateTime(2026, 10, 13); // Thứ Ba


            bool result = strategy.RequiresReorder(item, tuesdayDate);


            Assert.False(result);
        }

        [Fact]
        public void PeriodicReview_OnReviewDayButQuantityAtOrAboveTarget_ReturnsFalse()
        {

            var item = CreateItem(quantity: 25);
            var strategy = new PeriodicReviewStrategy(DayOfWeek.Monday);
            var mondayDate = new DateTime(2026, 10, 12);


            bool result = strategy.RequiresReorder(item, mondayDate);


            Assert.False(result);
        }

        [Fact]
        public void PeriodicReview_CalculateReorderQuantity_ReturnsDifferenceTargetMinusCurrent()
        {

            var item =CreateItem(quantity: 10);
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