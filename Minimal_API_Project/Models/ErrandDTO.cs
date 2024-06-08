namespace Minimal_API_Project.Models
{
    public class ErrandDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsCompleted { get; set; }
        public string? Description { get; set; }
    }
}
