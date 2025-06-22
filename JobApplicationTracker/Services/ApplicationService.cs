using DatabaseContext;
using JobApplicationTracker.API.Models;
using JobApplicationTracker.API.Models.Mapper;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.API.Services
{
    public class ApplicationService: IApplicationService
    {
        private readonly AppDatabaseContext _context;
        public ApplicationService(AppDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Application>> GetAllApplicationsAsync()
        {
            var entities = await _context.Applications.ToListAsync();
            return entities.MapToApplication();
        }

        public async Task<Application?> GetApplicationByIdAsync(int applicationId)
        {
            if (applicationId <= 0)
                return null;
            var entity = await _context.Applications.FindAsync(applicationId);
            return entity?.ToModel();
        }

        public async Task<Application> AddApplicationAsync(Application application)
        {
            if (application == null)
                throw new ArgumentNullException(nameof(application));
            var entity = application.ToEntity();
            await _context.Applications.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.ToModel();
        }

        public async Task<Application> UpdateApplicationAsync(Application application)
        {
            if (application == null)
                throw new ArgumentNullException(nameof(application));
            var existingEntity = await _context.Applications.FindAsync(application.Id);
            if (existingEntity == null)
                throw new KeyNotFoundException($"Application with ID {application.Id} not found.");
            existingEntity.JobTitle = application.JobTitle;
            existingEntity.CompanyName = application.CompanyName;
            existingEntity.ApplicationDate = application.ApplicationDate;
            existingEntity.Status = (int)application.Status;
            existingEntity.Notes = application.Notes;
            await _context.SaveChangesAsync();
            return existingEntity.ToModel();
        }

        public async Task DeleteApplicationAsync(int applicationId)
        {
            if (applicationId <= 0)
                throw new ArgumentException("Invalid application ID.", nameof(applicationId));
            var entity = await _context.Applications.FindAsync(applicationId);
            if (entity == null)
                throw new KeyNotFoundException($"Application with ID {applicationId} not found.");
            _context.Applications.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}

