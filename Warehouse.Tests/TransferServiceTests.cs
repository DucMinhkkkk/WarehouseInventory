using System;
using System.Linq;
using Xunit;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Exceptions;
using Warehouse.Infrastructure.Services;

namespace Warehouse.Tests
{
    public class TransferServiceTests
    {
        private readonly Product _product =
        new Product("SKU-100", "Test Product", "");

        private readonly Product _differentProduct =
            new Product("SKU-999", "Different Product", "");

        private readonly Location _locA =
            new Location("LOC-A", "Zone A");

        private readonly Location _locB =
            new Location("LOC-B", "Zone B");

        [Fact]
        public void TransferStock_NullSourceOrDestination_ThrowsInvalidStockOperationException()
        {
            var logger = new AuditLogger();
            var service = new TransferService(logger);
            var validSource = new StockItem("STOCK-1", _product, _locA, initialQuantity: 10);

            Assert.Throws<InvalidStockOperationException>(() => 
                service.TransferStock(null, validSource, 5, "User1"));
            
            Assert.Throws<InvalidStockOperationException>(() => 
                service.TransferStock(validSource, null, 5, "User1"));
        }

        [Fact]
        public void TransferStock_MismatchedSkus_ThrowsInvalidStockOperationException()
        {
            var logger = new AuditLogger();
            var service = new TransferService(logger);
            
            var source = new StockItem("STOCK-1", _product, _locA, initialQuantity: 10);
            var destination = new StockItem("STOCK-2", _differentProduct, _locB, initialQuantity: 5);

            var exception = Assert.Throws<InvalidStockOperationException>(() => 
                service.TransferStock(source, destination, 5, "User1"));
                
            Assert.Equal("Cannot transfer stock between different SKUs.", exception.Message);
        }

        [Fact]
        public void TransferStock_SuccessfulTransfer_UpdatesQuantitiesAndLogs()
        {
            var logger = new AuditLogger();
            var service = new TransferService(logger);

            var source = new StockItem(
                "STOCK-1", _product, _locA, initialQuantity: 20);

            var destination = new StockItem(
                "STOCK-2", _product, _locB, initialQuantity: 5);

            service.TransferStock(source, destination, 10, "User123");

            // Kiểm tra số lượng sau khi chuyển
            Assert.Equal(10, source.Quantity);
            Assert.Equal(15, destination.Quantity);

            // Kiểm tra nhật ký
            var logs = logger.GetHistory();

            Assert.Single(logs);
            Assert.Equal("TRANSFER", logs.First().ActionType);
            Assert.Equal(10, logs.First().Quantity);
            Assert.Equal("User123", logs.First().UserId);
        }
    }
}