namespace MiApisBeer.DTO
{
    public class ProveedoreUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Ruc { get; set; }
        public string? Telefono { get; set; }
    }
}
