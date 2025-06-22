namespace JobApplicationTracker.API.Models
{
    /// <summary>
    /// Represents a job application, including details such as the job title, company name, application date, status,
    /// and additional notes.
    /// </summary>
    public class Application
    {
        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the job title.
        /// </summary>
        public string JobTitle { get; set; }

        /// <summary>
        /// Gets or sets the Company Name.
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the date of the job application.
        /// </summary>
        public DateTime ApplicationDate { get; set; }

        /// <summary>
        /// Gets or sets the current status of the job application.
        /// </summary>
        public ApplicationStatus Status { get; set; } 

        /// <summary>
        /// Gets or sets the notes 
        /// </summary>
        public string Notes { get; set; }
    }
}
