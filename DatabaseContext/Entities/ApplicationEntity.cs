namespace DatabaseContext.Entities
{
    public class ApplicationEntity
    {
        public int Id { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public DateTime ApplicationDate { get; set; }
        public int Status { get; set; } // Store enum as int
        public string Notes { get; set; } = string.Empty;
    }
}