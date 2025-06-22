using JobApplicationTracker.API.Models;

namespace JobApplicationTracker.API.Models.Mapper
{
    public static class ApplicationViewModelMapper
    {
        public static Application ToModel(this ApplicationViewModel viewModel)
        {
            if (viewModel == null) return null;
            return new Application
            {
                Id = viewModel.Id,
                JobTitle = viewModel.JobTitle,
                CompanyName = viewModel.CompanyName,
                ApplicationDate = viewModel.ApplicationDate,
                Status = Enum.TryParse<ApplicationStatus>(viewModel.Status, out var status) ? status : ApplicationStatus.Applied,
                Notes = viewModel.Notes
            };
        }

        public static ApplicationViewModel ToViewModel(this Application application)
        {
            if (application == null) return null;
            return new ApplicationViewModel
            {
                Id = application.Id,
                JobTitle = application.JobTitle,
                CompanyName = application.CompanyName,
                ApplicationDate = application.ApplicationDate,
                Status = application.Status.ToString(),
                Notes = application.Notes
            };
        }

        public static IEnumerable<Application> MapToApplication(this IEnumerable<ApplicationViewModel> viewModels)
            => viewModels?.Select(vm => vm.ToModel()) ?? Enumerable.Empty<Application>();

        public static IEnumerable<ApplicationViewModel> MapToViewModel(this IEnumerable<Application> models)
            => models?.Select(m => m.ToViewModel()) ?? Enumerable.Empty<ApplicationViewModel>();
    }
}