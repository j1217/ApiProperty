namespace ApiProperty.Models.DTO
{
    public class PropertyTraceInfo
    {
        public int IdPropertyTrace { get; set; }
        public DateTime? DataSale { get; set; }
        public string Name { get; set; }
        public decimal? Value { get; set; }
        public decimal? Tax { get; set; }
    }

}
