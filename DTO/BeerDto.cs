namespace MiApisBeer.DTO
{
    public class BeerDto
    {
        public int BeerId { get; set; } 
        public string Name { get; set; } = null!;
        public int BrandId { get; set; }
        public string BrandName { get; set; } = null!;
    }
}
