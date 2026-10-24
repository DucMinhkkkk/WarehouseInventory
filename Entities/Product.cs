namespace Warehouse.Domain.Entities
{
    public class Product
    {
        public string SKU { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public Product(string sku, string name, string description = "")
        {
            SKU = sku;
            Name = name;
            Description = description;
        }
    }
}