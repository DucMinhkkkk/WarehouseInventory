Sơ đồ UML cho tới tuần này:

classDiagram
    namespace Domain_Entities {
        class StockItem {
            +string Sku
            +string LocationId
            +int Quantity
            +int ReorderPoint
            +int TargetStockLevel
            +AddStock(int amount)
            +DeductStock(int amount)
            +UpdateReorderPolicy(int reorderPoint, int targetStockLevel)
        }
        class Product {
            +string Sku
            +string Name
            +decimal Price
        }
        class Location {
            +string LocationId
            +string Name
        }
        class Shipment {
            +string ShipmentId
            +ShipmentType Type
            +DateTime CreatedDate
        }
        class ShipmentType {
            <<enumeration>>
            Inbound
            Outbound
        }
        class AuditEntry {
            +DateTime Timestamp
            +string Operator
            +string Action
            +string Sku
            +int QuantityDelta
            +string LocationId
        }
    }

    namespace Domain_Exceptions {
        class Exception {
            <<System>>
        }
        class InvalidStockOperationException {
        }
        class InsufficientStockException {
        }
    }

    namespace Domain_Strategies {
        class IReplenishmentStrategy {
            <<interface>>
            +string StrategyName
            +RequiresReorder(StockItem item, DateTime currentDate) bool
            +CalculateReorderQuantity(StockItem item) int
        }
        class FixedReorderPointStrategy {
            +string StrategyName
            +RequiresReorder(StockItem item, DateTime currentDate) bool
            +CalculateReorderQuantity(StockItem item) int
        }
        class PeriodicReviewStrategy {
            -DayOfWeek _reviewDay
            +string StrategyName
            +RequiresReorder(StockItem item, DateTime currentDate) bool
            +CalculateReorderQuantity(StockItem item) int
        }
    }

    namespace Infrastructure {
        class AuditLogger {
            -List~AuditEntry~ _logs
            +Log(string operatorName, string action, string sku, int delta, string locationId)
            +GetLogs() IReadOnlyList~AuditEntry~
        }
        class TransferService {
            -AuditLogger _auditLogger
            +TransferStock(StockItem source, StockItem target, int quantity, string operatorName)
        }
        class LowStockReportGenerator {
            +GenerateReport(IEnumerable~StockItem~ items, IReplenishmentStrategy strategy, DateTime currentDate) List~LowStockReportItem~
        }
        class LowStockReportItem {
            +string Sku
            +string LocationId
            +int CurrentQuantity
            +int RecommendedReorderQuantity
            +string StrategyUsed
        }
    }

    %% Relationships
    Exception <|-- InvalidStockOperationException
    InvalidStockOperationException <|-- InsufficientStockException

    Shipment --> ShipmentType

    IReplenishmentStrategy <|.. FixedReorderPointStrategy
    IReplenishmentStrategy <|.. PeriodicReviewStrategy

    AuditLogger "1" *-- "*" AuditEntry : contains
    TransferService --> AuditLogger : uses
    TransferService ..> StockItem : manipulates
    
    LowStockReportGenerator ..> StockItem : evaluates
    LowStockReportGenerator ..> IReplenishmentStrategy : relies on
    LowStockReportGenerator ..> LowStockReportItem : creates
