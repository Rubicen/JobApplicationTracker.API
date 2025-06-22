using DatabaseContext.Entities;
using JobApplicationTracker.API.Models;

namespace JobApplicationTracker.API.Models.Mapper
{
    public static class ApplicationEntityMapper
    {
        public static Application ToModel(this ApplicationEntity entity)
        {
            if (entity == null) return null;
            return new Application
            {
                Id = entity.Id,
                JobTitle = entity.JobTitle,
                CompanyName = entity.CompanyName,
                ApplicationDate = entity.ApplicationDate,
                Status = (ApplicationStatus)entity.Status,
                Notes = entity.Notes
            };
        }

        public static ApplicationEntity ToEntity(this Application model)
        {
            if (model == null) return null;
            return new ApplicationEntity
            {
                Id = model.Id,
                JobTitle = model.JobTitle,
                CompanyName = model.CompanyName,
                ApplicationDate = model.ApplicationDate,
                Status = (int)model.Status,
                Notes = model.Notes
            };
        }

        public static IEnumerable<Application> MapToApplication(this IEnumerable<ApplicationEntity> entities)
            => entities?.Select(e => e.ToModel()) ?? Enumerable.Empty<Application>();

        public static IEnumerable<ApplicationEntity> MapToEntity(this IEnumerable<Application> models)
            => models?.Select(m => m.ToEntity()) ?? Enumerable.Empty<ApplicationEntity>();
    }
}