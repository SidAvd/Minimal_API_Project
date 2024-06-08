namespace Minimal_API_Project.Models
{
    public class ErrandWorker
    {
        public Errand Errand { get; set; }
        public int ErrandId { get; set; }   // Foreign key
        public Worker Worker { get; set; }
        public int WorkerId { get; set; }   // Foreign key
    }
}
