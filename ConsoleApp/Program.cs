using System;
using System.Collections.Generic;
using System.Text;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Strategy;
using Warehouse.Infrastructure.Reporting;
using Warehouse.Infrastructure.Services;

namespace WarehouseInventory.ConsoleApp;

class Program
{
    static void Main(string[] args)
    {   
        Console.OutputEncoding = Encoding.UTF8; //Đảm bảo hiển thị Tiếng Việt có dấu trên Windows Terminal / Command Prompt

        PrintHeader("HỆ THỐNG QUẢN LÝ MẶT HÀNG & TÁI MẶT HÀNG (WAREHOUSE )");

        var auditLogger = new AuditLogger();
        var transferService = new TransferService(auditLogger);
        var reportGenerator = new LowStockReprotGenerator();

        //Kịch Bản 1: Happy path (Luân chuyển kho nguyên tử & Ghi Audit Log)
        PrintSection("1. KỊCH BẢN HAPPY PATH: LUÂN CHUYỂN KHO NGUYỂN TỬ & AUDIT TRAIL");

        var khoMain = new StockItem("DELL-XPS-15", "KHO-MAIN", initialQuantity: 15,reorderPoint: 10, targetStockLevel: 25);
        var khoSub = new StockItem("DELL-XPS-15", "KHO-SUB", initialQuantity: 5, reorderPoint: 10, targetStockLevel: 25);

        Console.WriteLine("[Trạng thái ban đầu]");
        Console.WriteLine($"  • Kho Chính (KHO-MAIN): {khoMain.Quantity} máy");
        Console.WriteLine($"  • Kho Phụ   (KHO-SUB) : {khoSub.Quantity} máy");

        Console.WriteLine("\n[Thao tác]: Chuyển 5 máy DELL-XPS-15 từ KHO-MAIN sang KHO-SUB bởi 'NV_NguyenVanA'...");
        transferService.TransferStock(khoMain, khoSub, quantity: 5, operatorName: "NV_NguyenVanA");

        Console.WriteLine("\n[Trạng thái sau khi chuyển]");
        Console.WriteLine($"  • Kho Chính (KHO-MAIN): {khoMain.Quantity} máy (Đã giảm 5)");
        Console.WriteLine($"  • Kho Phụ   (KHO-SUB) : {khoSub.Quantity} máy (Đã tăng 5)");

        Console.WriteLine("\n[Nhật ký dấu vết hệ thống (Audit Trail)]");
        PrintAuditLogs(auditLogger.GetLogs());

        //Kịch bản 2: VALIDATION & ERROR HANDLING (Chống xuất âm kho & Rollback)
        PrintSection("2. KỊCH BẢN BẢO VỆ: CHỐNG XUẤT ÂM KHO (OVERDRAFT PREVENTION)");
        Console.WriteLine($"[Trạng thái hiện tại] Kho Chính (KHO-MAIN) đang có {khoMain.Quantity} máy.");
        Console.WriteLine("[Thao tác lỗi]: Cố gắng chuyển 20 máy từ KHO-MAIN sang KHO-SUB (vượt quá tồn kho)...");

        try
        {
            transferService.TransferStock(khoMain, khoSub, quantity: 20, operatorName: "NV_NguyenVanA");

        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n[BẮT LỖI THÀNH CÔNG]: {ex.GetType().Name} - {ex.Message}");
            Console.ResetColor();
        }
        Console.WriteLine("\n[Xác minh tính Nguyên tử (Atomic Integrity)]:");
        Console.WriteLine($"  • Kho Chính (KHO-MAIN) vẫn giữ nguyên: {khoMain.Quantity} máy (Không bị trừ dở chừng)");
        Console.WriteLine($"  • Kho Phụ   (KHO-SUB)  vẫn giữ nguyên: {khoSub.Quantity} máy (Không bị cộng oan)");
        

        //Kịch bản 3: STRATEGY PATTERN & POLYMORPHISM (So sánh 2 thuật toán Tái đặt hàng)
        PrintSection("3. KỊCH BẢN ĐA HÌNH (POLYMORPHISM): SO SÁNH 2 STRATEGY TÁI ĐẶT HÀNG");

        // Giả lập sản phẩm chạm vạch nguy cơ: Tồn kho = 8 (thấp hơn ReorderPoint = 10)
        var itemNeedReorder = new StockItem("DELL-XPS-15", "KHO-MAIN", initialQuantity: 8, reorderPoint: 10, targetStockLevel: 25);
        var stockList = new List<StockItem> { itemNeedReorder };

        // Giả lập thời điểm đánh giá là THỨ BA (Tuesday)
        var evaluatedDate = new DateTime(2026, 10, 6); // 06/10/2026 là Thứ Ba

        Console.WriteLine($"Sản phẩm kiểm tra : SKU '{itemNeedReorder.Sku}' tại '{itemNeedReorder.LocationId}'");
        Console.WriteLine($"Thông số Tồn kho   : Tồn thực tế = {itemNeedReorder.Quantity} | Ngưỡng đặt (ROP) = {itemNeedReorder.ReorderPoint} | Mức tối đa (Target) = {itemNeedReorder.TargetStockLevel}");
        Console.WriteLine($"Ngày đánh giá     : {evaluatedDate:dd/MM/yyyy} ({evaluatedDate.DayOfWeek})\n");
        
        // --- CHIẾN LƯỢC 1: Fixed Reorder Point (Kiểm tra liên tục) ---
        IReplenishmentStrategy fixedStrategy = new FixedReorderPointStrategy();
        var report1 = reportGenerator.GenerateReport(stockList, fixedStrategy,evaluatedDate);

        Console.WriteLine($"▶ Chiến lược 1: {fixedStrategy.StrategyName}");
        PrintReportResults(report1);

        // ---Chiến lược 2: Preiodic Review (Chỉ review vào Thứ Hai) ---
        IReplenishmentStrategy periodicStrategy = new PeriodicReviewStrategy(DayOfWeek.Monday);
        var report2 = reportGenerator.GenerateReport(stockList, periodicStrategy,evaluatedDate);

        Console.WriteLine($"▶ Chiến lược 2: {periodicStrategy.StrategyName}");
        PrintReportResults(report2);

        Console.WriteLine("----------------------------------------------------------------------------------");
        Console.WriteLine("=> KẾT LUẬN DEMO TÍNH ĐA HÌNH:");
        Console.WriteLine("   • Cùng dữ liệu Tồn kho (= 8), nhưng ở ngày Thứ Ba:");
        Console.WriteLine("     + FixedReorderPointStrategy : KÍCH HOẠT NHẬP HÀNG (vì 8 <= 10).");
        Console.WriteLine("     + PeriodicReviewStrategy    : BỎ QUA (vì Thứ Ba không phải Thứ Hai theo lịch).");

        PrintHeader("HOÀN THÀNH KỊCH BẢN DEMO Console - ĐỦ ĐIỀU KIỆN ĐÁNH GIÁ ĐỒ ÁN");

    }

    private static void PrintHeader(string title)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n==================================================================================");
        Console.WriteLine($"   {title}");
        Console.WriteLine("==================================================================================");
        Console.ResetColor();
    }

    private static void PrintSection(string sectionTitle)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n----------------------------------------------------------------------------------");
        Console.WriteLine($" {sectionTitle}");
        Console.WriteLine($"----------------------------------------------------------------------------------");
        Console.ResetColor();
    }

    private static void PrintAuditLogs(IEnumerable<AuditEntry> logs)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        foreach (var log in logs)
        {
            Console.WriteLine($"  [{log.Timestamp:HH:mm:ss}] Operator: {log.Operator,-15} | Action: {log.Action,-12} | SKU: {log.Sku,-12} | ΔQty: {log.QuantityDelta,3} | Loc: {log.LocationId}");
        }
        Console.ResetColor();
    }

    private static void PrintReportResults(List<LowStockReportItem> report)
    {
        if (report.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("   [Kết quả Báo cáo]: KHÔNG KHUYẾN NGHỊ ĐẶT HÀNG (0 mặt hàng).\n");
            Console.ResetColor();
            return;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"   [Kết quả Báo cáo]: PHÁT HIỆN {report.Count} MẶT HÀNG CẦN TÁI ĐẶT HÀNG!");
        foreach (var item in report)
        {
            Console.WriteLine($"   • SKU: {item.Sku,-12} | Vị trí: {item.LocationId,-10} | Tồn hiện tại: {item.CurrentQuantity,2} | Đề xuất nhập thêm: {item.RecommendedReorderQuantity,2}");
        }
        Console.WriteLine();
        Console.ResetColor();
    }

}