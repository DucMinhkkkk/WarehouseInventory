WarehouseInventory - .NET project setup files

This package contains project/solution configuration files only.
It does NOT contain your repository's existing C# source files, and it does
not fix the known namespace/API/compile errors.

Target framework: .NET 8 (net8.0)

How to apply:
1. Download/clone your repository:
   git clone https://github.com/DucMinhkkkk/WarehouseInventory.git
2. Copy these files into the matching paths in the repository:
   - Warehouse.Core.csproj -> repository root
   - WarehouseInventory.sln -> repository root
   - ConsoleApp/ConsoleApp.csproj -> existing ConsoleApp folder
   - Warehouse.Tests/Warehouse.Tests.csproj -> existing Warehouse.Tests folder
3. From repository root, run:
   dotnet restore WarehouseInventory.sln
   dotnet build WarehouseInventory.sln
   dotnet test WarehouseInventory.sln

Important:
- Warehouse.Core.csproj includes the C# source files at repository root and in
  existing domain/service folders, excluding ConsoleApp and Warehouse.Tests.
- Build/test may still fail until the existing source code's namespace and
  member-name inconsistencies are fixed.
- The files in this package have not been validated by running dotnet build/test
  against the repository in this environment.
