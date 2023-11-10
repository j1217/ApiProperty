using ApiProperty.Models.Domain;

namespace ApiProperty.Models.DTO
{
    public class PropertyInfo
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal Price { get; set; }
        public string CodeInternal { get; set; }
        public int Year { get; set; }
        public string OwnerName { get; set; }
        public List<PropertyImage> PropertyImages { get; set; }
        public List<PropertyTraceInfo> PropertyTraces { get; set; }
    }

}
