using System.ComponentModel.DataAnnotations;

namespace JobApplicationTracker.API.Models.Dto
{
    public class ApplicationUpdateDto
    {
        public int Id { get; set; }

        public string JobTitle { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;

        [Required]
        public DateTime ApplicationDate { get; set; }

        [Required]
        public string Status { get; set; }

        public string? Notes { get; set; }
    }
}