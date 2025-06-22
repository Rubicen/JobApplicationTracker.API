using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobApplicationTracker.API.Controllers;
using JobApplicationTracker.API.Models;
using JobApplicationTracker.API.Models.Dto;
using JobApplicationTracker.API.Models.Mapper;
using JobApplicationTracker.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace JobApplicationTracker.Tests
{
    public class ApplicationsControllerTests
    {
        private readonly Mock<ILogger<ApplicationsController>> _loggerMock;
        private readonly Mock<IApplicationService> _serviceMock;
        private readonly ApplicationsController _controller;

        public ApplicationsControllerTests()
        {
            _loggerMock = new Mock<ILogger<ApplicationsController>>();
            _serviceMock = new Mock<IApplicationService>();
            _controller = new ApplicationsController(_loggerMock.Object, _serviceMock.Object);
        }

        //Get tests
        [Fact]
        public async Task Get_ReturnsAllApplications()
        {
            // Arrange
            var applications = new List<Application>
            {
                new Application { Id = 1, JobTitle = "Dev", CompanyName = "A" },
                new Application { Id = 2, JobTitle = "QA", CompanyName = "B" }
            };
            _serviceMock.Setup(s => s.GetAllApplicationsAsync()).ReturnsAsync(applications);

            // Act
            var result = await _controller.Get();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        //Get by Id tests
        [Fact]
        public async Task Get_ById_ReturnsApplication_WhenFound()
        {
            // Arrange
            var application = new Application { Id = 1, JobTitle = "Dev", CompanyName = "A" };
            _serviceMock.Setup(s => s.GetApplicationByIdAsync(1)).ReturnsAsync(application);

            // Act
            var result = await _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var viewModel = Assert.IsType<ApplicationViewModel>(okResult.Value);
            Assert.Equal(1, viewModel.Id);
        }

        [Fact]
        public async Task Get_ById_ReturnsBadRequest_WhenIdInvalid()
        {
            // Act
            var result = await _controller.Get(0);

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task Get_ById_ReturnsNotFound_WhenNotFound()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetApplicationByIdAsync(99)).ReturnsAsync(null as Application);

            // Act
            var result = await _controller.Get(99);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        //Post tests
        [Fact]
        public async Task Post_ReturnsCreated_WhenValid()
        {
            // Arrange
            var dto = new ApplicationCreateDto
            {
                JobTitle = "Dev",
                CompanyName = "A",
                ApplicationDate = DateTime.UtcNow,
                Status = "Applied",
                Notes = "note"
            };
            var model = new Application
            {
                Id = 1,
                JobTitle = dto.JobTitle,
                CompanyName = dto.CompanyName,
                ApplicationDate = dto.ApplicationDate,
                Status = ApplicationStatus.Applied,
                Notes = dto.Notes
            };
            _serviceMock.Setup(s => s.AddApplicationAsync(It.IsAny<Application>())).ReturnsAsync(model);

            // Act
            var result = await _controller.Post(dto);

            // Assert
            var created = Assert.IsType<CreatedAtActionResult>(result);
            var viewModel = Assert.IsType<ApplicationViewModel>(created.Value);
            Assert.Equal(1, viewModel.Id);
        }

        [Fact]
        public async Task Post_ReturnsBadRequest_WhenDtoNull()
        {
            // Act
            var result = await _controller.Post(null);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Application cannot be null.", badRequest.Value);
        }

        [Fact]
        public async Task Post_ReturnsBadRequest_WhenStatusInvalid()
        {
            // Arrange
            var dto = new ApplicationCreateDto
            {
                JobTitle = "Dev",
                CompanyName = "A",
                ApplicationDate = DateTime.UtcNow,
                Status = "InvalidStatus"
            };

            // Act
            var result = await _controller.Post(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        }

        //Put tests
        [Fact]
        public async Task Put_ReturnsOk_WhenValid()
        {
            // Arrange
            var dto = new ApplicationUpdateDto
            {
                Id = 1,
                JobTitle = "Dev",
                CompanyName = "A",
                ApplicationDate = DateTime.UtcNow,
                Status = "Applied",
                Notes = "note"
            };
            var model = new Application
            {
                Id = 1,
                JobTitle = dto.JobTitle,
                CompanyName = dto.CompanyName,
                ApplicationDate = dto.ApplicationDate,
                Status = ApplicationStatus.Applied,
                Notes = dto.Notes
            };
            _serviceMock.Setup(s => s.UpdateApplicationAsync(It.IsAny<Application>())).ReturnsAsync(model);

            // Act
            var result = await _controller.Put(dto);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var viewModel = Assert.IsType<ApplicationViewModel>(ok.Value);
            Assert.Equal(1, viewModel.Id);
        }

        [Fact]
        public async Task Put_ReturnsBadRequest_WhenDtoNull()
        {
            // Act
            var result = await _controller.Put(null);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Application cannot be null.", badRequest.Value);
        }

        [Fact]
        public async Task Put_ReturnsBadRequest_WhenStatusInvalid()
        {
            // Arrange
            var dto = new ApplicationUpdateDto
            {
                Id = 1,
                JobTitle = "Dev",
                CompanyName = "A",
                ApplicationDate = DateTime.UtcNow,
                Status = "InvalidStatus"
            };

            // Act
            var result = await _controller.Put(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Put_ReturnsNotFound_WhenKeyNotFound()
        {
            // Arrange
            var dto = new ApplicationUpdateDto
            {
                Id = 99,
                JobTitle = "Dev",
                CompanyName = "A",
                ApplicationDate = DateTime.UtcNow,
                Status = "Applied"
            };
            _serviceMock.Setup(s => s.UpdateApplicationAsync(It.IsAny<Application>()))
                .ThrowsAsync(new KeyNotFoundException("Not found"));

            // Act
            var result = await _controller.Put(dto);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Not found", notFound.Value);
        }

        //Delete tests
        [Fact]
        public async Task Delete_ReturnsNoContent_WhenValid()
        {
            // Arrange
            _serviceMock.Setup(s => s.DeleteApplicationAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsBadRequest_WhenIdInvalid()
        {
            // Act
            var result = await _controller.Delete(0);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid application ID.", badRequest.Value);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenKeyNotFound()
        {
            // Arrange
            _serviceMock.Setup(s => s.DeleteApplicationAsync(99))
                .ThrowsAsync(new KeyNotFoundException("Not found"));

            // Act
            var result = await _controller.Delete(99);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Not found", notFound.Value);
        }

        // Logging tests
        [Fact]
        public async Task Get_ById_LogsWarning_WhenIdInvalid()
        {
            // Act
            await _controller.Get(0);

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("invalid applicationId")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
