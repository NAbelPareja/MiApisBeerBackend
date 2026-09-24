namespace MiApisBeer.DTO
{
    public class ProveedoreUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Address { get; set; }
        public string? Phone { get; set; }
    }
}
