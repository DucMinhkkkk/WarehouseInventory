namespace Warehouse.Domain.Entities
{
    public class Location
    {
        public string LocationId { get; private set; }
        public string Name { get; private set; }

        public Location(string locationId, string name)
        {
            LocationId = locationId;
            Name = name;
        }
    }
}