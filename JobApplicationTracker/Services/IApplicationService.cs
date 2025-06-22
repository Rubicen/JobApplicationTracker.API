using JobApplicationTracker.API.Models;

namespace JobApplicationTracker.API.Services
{
    public interface IApplicationService
    {
        Task<IEnumerable<Application>> GetAllApplicationsAsync();
        Task<Application?> GetApplicationByIdAsync(int applicationId);
        Task<Application> AddApplicationAsync(Application application);
        Task<Application> UpdateApplicationAsync(Application application);
        Task DeleteApplicationAsync(int applicationId);
    }
}
