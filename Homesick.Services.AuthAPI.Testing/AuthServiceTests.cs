using Homesick.Services.AuthAPI.Controllers;
using Homesick.Services.AuthAPI.Models.DTO;
using Homesick.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Homesick.Services.AuthAPI.Testing
{
    public class AuthServiceTests
    {
        private readonly AuthAPIController _controller;
        private readonly Mock<IAuthService> _mockAuthService;

        public AuthServiceTests()
        {
            _mockAuthService = new Mock<IAuthService>();
            _controller = new AuthAPIController(_mockAuthService.Object);
        }

        [Fact]
        public async Task Register_ReturnsError_WhenRegistrationFails()
        {
            // Arrange
            var model = new RegistrationRequestDto { Email = "test@example.com", Password = "pass" };
            _mockAuthService.Setup(x => x.Register(model)).ReturnsAsync("Email already exists");

            // Act
            var result = await _controller.Register(model);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Email already exists", result.Message);
        }

        [Fact]
        public async Task Register_ReturnsSuccess_WhenRegistrationSucceeds()
        {
            // Arrange
            var model = new RegistrationRequestDto { Email = "new@example.com", Password = "pass" };
            _mockAuthService.Setup(x => x.Register(model)).ReturnsAsync(string.Empty);

            // Act
            var result = await _controller.Register(model);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Message);
        }

        [Fact]
        public async Task Login_ReturnsBadRequest_WhenLoginFails()
        {
            // Arrange
            var model = new LoginRequestDto { UserName = "user@example.com", Password = "wrong" };
            _mockAuthService.Setup(x => x.Login(model)).ReturnsAsync(new LoginResponseDto { User = null });

            // Act
            var result = await _controller.Login(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ResponseDto>(badRequestResult.Value);
            Assert.False(response.IsSuccess);
            Assert.Equal("Username or password is incorrect", response.Message);
        }

        [Fact]
        public async Task Login_ReturnsOk_WhenLoginSucceeds()
        {
            // Arrange
            var model = new LoginRequestDto { UserName = "user@example.com", Password = "correct" };
            var loginResponse = new LoginResponseDto
            {
                User = new UserDto { Email = "user@example.com" },
                Token = "some-jwt-token"
            };

            _mockAuthService.Setup(x => x.Login(model)).ReturnsAsync(loginResponse);

            // Act
            var result = await _controller.Login(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseDto>(okResult.Value);
            Assert.True(response.IsSuccess);
            Assert.Equal(loginResponse, response.Result);
        }

        [Fact]
        public async Task AssignRole_ReturnsBadRequest_WhenFails()
        {
            // Arrange
            var model = new RegistrationRequestDto { Email = "admin@example.com", Role = "Διαχειριστής" };
            _mockAuthService.Setup(x => x.AssignRole(model.Email, model.Role.ToUpper())).ReturnsAsync(false);

            // Act
            var result = await _controller.AssignRole(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ResponseDto>(badRequestResult.Value);
            Assert.False(response.IsSuccess);
            Assert.Equal("Error encountered", response.Message);
        }

        [Fact]
        public async Task AssignRole_ReturnsOk_WhenSuccess()
        {
            // Arrange
            var model = new RegistrationRequestDto { Email = "admin@example.com", Role = "Διαχειριστής" };
            _mockAuthService.Setup(x => x.AssignRole(model.Email, model.Role.ToUpper())).ReturnsAsync(true);

            // Act
            var result = await _controller.AssignRole(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseDto>(okResult.Value);
            Assert.True(response.IsSuccess);
        }
    }
}