using JobApplicationTracker.API.Models;
using JobApplicationTracker.API.Models.Dto;
using JobApplicationTracker.API.Models.Mapper;
using JobApplicationTracker.API.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace JobApplicationTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationsController : ControllerBase
    {
        private readonly ILogger<ApplicationsController> _logger;
        private readonly IApplicationService _applicationService;

        public ApplicationsController(ILogger<ApplicationsController> logger, IApplicationService applicationService)
        {
            _logger = logger;
            _applicationService = applicationService;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Retrieve all applications",
            Description = "Fetches all job applications from the database.",
            Tags = new[] { "Applications" }
        )]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<ApplicationViewModel>))]
        public async Task<IEnumerable<ApplicationViewModel>> Get()
        {
            var applications = await _applicationService.GetAllApplicationsAsync();
            return applications.MapToViewModel();
        }

        [HttpGet("{applicationId}")]
        [SwaggerOperation(
            Summary = "Retrieve a specific application",
            Description = "Fetches a job application by its ID.",
            Tags = new[] { "Applications" }
        )]
        [SwaggerResponse(200, "Success", typeof(ApplicationViewModel))]
        [SwaggerResponse(400, "Invalid application ID")]
        [SwaggerResponse(404, "Application not found")]
        public async Task<ActionResult<ApplicationViewModel?>> Get(int applicationId)
        {
            if (applicationId <= 0)
            {
                _logger.LogWarning("Get called with invalid applicationId: {ApplicationId}", applicationId);
                return BadRequest();
            }

            _logger.LogInformation("Fetching application with ID {ApplicationId}.", applicationId);
            Application? application = await _applicationService.GetApplicationByIdAsync(applicationId);

            if (application == null)
            {
                _logger.LogWarning("Application with ID {ApplicationId} not found.", applicationId);
                return NotFound();
            }

            return Ok(application.ToViewModel());
        }

        [HttpPost()]
        [SwaggerOperation(
            Summary = "Create a new application",
            Description = "Creates a new job application in the database.",
            Tags = new[] { "Applications" }
        )]
        [SwaggerResponse(201, "Application created", typeof(ApplicationViewModel))]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<IActionResult> Post([FromBody] ApplicationCreateDto applicationCreateDto)
        {
            if (applicationCreateDto == null)
            {
                _logger.LogWarning("Attempted to create application with null DTO.");
                return BadRequest("Application cannot be null.");
            }
            if (!Enum.TryParse<ApplicationStatus>(applicationCreateDto.Status, out var status))
            {
                _logger.LogWarning("Invalid status value '{Status}' provided in create DTO.", applicationCreateDto.Status);
                ModelState.AddModelError("Status", "Invalid status value.");
                return BadRequest(ModelState);
            }

            Application application = applicationCreateDto.ToModel();
            application = await _applicationService.AddApplicationAsync(application);

            return CreatedAtAction(nameof(Get), new { application = application.Id }, application.ToViewModel());
        }

        [HttpPut()]
        [SwaggerOperation(
            Summary = "Update an application",
            Description = "Updates a job application in the database.",
            Tags = new[] { "Applications" }
        )]
        [SwaggerResponse(200, "Application updated", typeof(ApplicationViewModel))]
        [SwaggerResponse(400, "Invalid input")]
        [SwaggerResponse(404, "Application not found")]
        public async Task<IActionResult> Put([FromBody] ApplicationUpdateDto applicationUpdateDto)
        {
            if (applicationUpdateDto == null)
            {
                _logger.LogWarning("Attempted to update application with null DTO.");
                return BadRequest("Application cannot be null.");
            }
            if (!Enum.TryParse<ApplicationStatus>(applicationUpdateDto.Status, out var status))
            {
                _logger.LogWarning("Invalid status value '{Status}' provided in update DTO.", applicationUpdateDto.Status);
                ModelState.AddModelError("Status", "Invalid status value.");
                return BadRequest(ModelState);
            }
            Application application = applicationUpdateDto.ToModel();
            try
            {
                application = await _applicationService.UpdateApplicationAsync(application);
                return Ok(application.ToViewModel());
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Application with ID {ApplicationId} not found for update.", application.Id);
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{applicationId}")]
        [SwaggerOperation(
            Summary = "Delete an application",
            Description = "Deletes a job application from the database based on ID.",
            Tags = new[] { "Applications" }
        )]
        [SwaggerResponse(204, "Application deleted")]
        [SwaggerResponse(400, "Invalid application ID")]
        [SwaggerResponse(404, "Application not found")]
        public async Task<IActionResult> Delete(int applicationId)
        {
            if (applicationId <= 0)
            {
                _logger.LogWarning("Attempted to delete application with invalid ID: {ApplicationId}", applicationId);
                return BadRequest("Invalid application ID.");
            }
            try
            {
                await _applicationService.DeleteApplicationAsync(applicationId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Application with ID {ApplicationId} not found for deletion.", applicationId);
                return NotFound(ex.Message);
            }
        }
    }
}
