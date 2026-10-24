using System;
using Xunit;
using Warehouse.Domain.Entities;

namespace Warehouse.Tests
{
    public class StockItemTests
    {
        private readonly Product _dummyProduct = new Product { SKU = "SKU-123" }; 
        private readonly Location _dummyLocation = new Location { Name = "A1" };

        [Fact]
        public void Constructor_ValidInitialQuantity_SetsPropertiesCorrectly()
        {
            var stockItem = new StockItem("ITEM-1", _dummyProduct, _dummyLocation, 10);
            
            Assert.Equal(10, stockItem.Quantity);
        }

        [Fact]
        public void Constructor_NegativeInitialQuantity_ThrowsInvalidOperationException()
        {
            var exception = Assert.Throws<InvalidOperationException>(() => 
                new StockItem("ITEM-1", _dummyProduct, _dummyLocation, -5));
                
            Assert.Equal("Số lượng tồn kho ban đầu không thể âm.", exception.Message);
        }

        [Fact]
        public void AddStock_ValidAmount_IncreasesQuantity()
        {
            var stockItem = new StockItem("ITEM-1", _dummyProduct, _dummyLocation, 10);
            
            stockItem.AddStock(5);
            
            Assert.Equal(15, stockItem.Quantity);
        }

        [Fact]
        public void RemoveStock_ValidAmount_DecreasesQuantity()
        {
            var stockItem = new StockItem("ITEM-1", _dummyProduct, _dummyLocation, 10);
            
            stockItem.RemoveStock(4);
            
            Assert.Equal(6, stockItem.Quantity);
        }

        [Fact]
        public void RemoveStock_InsufficientStock_ThrowsInvalidOperationException()
        {
            var stockItem = new StockItem("ITEM-1", _dummyProduct, _dummyLocation, 5);
            
            var exception = Assert.Throws<InvalidOperationException>(() => 
                stockItem.RemoveStock(10));
                
            Assert.Contains("Không đủ hàng trong kho", exception.Message);
        }
    }
}