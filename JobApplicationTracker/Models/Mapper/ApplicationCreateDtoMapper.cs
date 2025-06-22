using JobApplicationTracker.API.Models;
using JobApplicationTracker.API.Models.Dto;

namespace JobApplicationTracker.API.Models.Mapper
{
    public static class ApplicationCreateDtoMapper
    {
        public static Application ToModel(this ApplicationCreateDto dto)
        {
            if (dto == null) return null!;
            return new Application
            {
                // Id is not set here, it will be handled by the database AutoIncrement
                JobTitle = dto.JobTitle,
                CompanyName = dto.CompanyName,
                ApplicationDate = dto.ApplicationDate,
                Status = Enum.TryParse<ApplicationStatus>(dto.Status, out var status) ? status : throw new ArgumentException($"Invalid status value: {dto.Status}"),
                Notes = dto.Notes ?? string.Empty
            };
        }
    }
}