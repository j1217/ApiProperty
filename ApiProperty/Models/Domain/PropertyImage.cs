namespace ApiProperty.Models.Domain
{
    public class PropertyImage
    {
        public int IdPropertyImage { get; set; }
        public byte[] FileProperty { get; set; }
        public bool Enable { get; set; }

        // Relación con Property (uno a uno)
        public int IdProperty { get; set; }
        public Property Property { get; set; }
    }
}
