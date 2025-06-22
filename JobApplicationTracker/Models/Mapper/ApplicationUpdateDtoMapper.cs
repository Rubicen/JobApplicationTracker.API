using JobApplicationTracker.API.Models;
using JobApplicationTracker.API.Models.Dto;

namespace JobApplicationTracker.API.Models.Mapper
{
    public static class ApplicationUpdateDtoMapper
    {
        public static Application ToModel(this ApplicationUpdateDto dto)
        {
            if (dto == null) return null!;
            return new Application
            {
                Id = dto.Id,
                Position = dto.Position,
                CompanyName = dto.CompanyName,
                ApplicationDate = dto.ApplicationDate,
                Status = Enum.TryParse<ApplicationStatus>(dto.Status, out var status) ? status : throw new ArgumentException($"Invalid status value: {dto.Status}"),
                Notes = dto.Notes ?? string.Empty
            };
        }
    }
}