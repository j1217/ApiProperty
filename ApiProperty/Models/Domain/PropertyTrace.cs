using System;

namespace ApiProperty.Models.Domain
{
    public class PropertyTrace
    {
        public int IdPropertyTrace { get; set; }
        public DateTime DataSale { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public decimal Tax { get; set; }

        // Relación con Property (uno a uno)
        public int IdProperty { get; set; }
        public Property Property { get; set; }
    }
}
