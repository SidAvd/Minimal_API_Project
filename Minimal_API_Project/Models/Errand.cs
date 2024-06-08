namespace Minimal_API_Project.Models
{
    public class Errand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsCompleted { get; set; }
        public int? DifficultyLevel { get; set; }               // Secret Information
        public string? Description { get; set; }
        public ICollection<ErrandWorker>? ErrandWorkers { get; set; }
    }
}
