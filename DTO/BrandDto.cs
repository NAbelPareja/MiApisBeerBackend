namespace MiApisBeer.DTO
{
    public class BrandDto
    {
        public int BrandId { get; set; }
        public string Name { get; set; } = null!;
        public int proveedorId { get; set; }
        public string ProveedorName { get; set; } = null!;
    }
}
