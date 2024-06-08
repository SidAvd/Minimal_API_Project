namespace Minimal_API_Project.Models
{
    public class Worker
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Age { get; set; }           // Secret Information
        public DateOnly? HireDate { get; set; }
        public ICollection<ErrandWorker>? ErrandWorkers { get; set; }
    }
}
