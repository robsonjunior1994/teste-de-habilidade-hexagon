using Hexagon.Api.Domain.Models;
using Hexagon.Api.Domain.Services.Interfaces;
using Hexagon.Api.Presentation.Common;
using Hexagon.Api.Presentation.DTOs;
using Hexagon.Api.Presentation.DTOs.Request;
using Hexagon.Api.Presentation.DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hexagon.Api.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        public UserController(IUserService userService, 
            IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserRequestDTO userDTO)
        {
            ResponseDTO response = new ResponseDTO();

            if (!ModelState.IsValid)
            {
                response.IsFailure("Invalid user data.", StatusCodes.Status400BadRequest.ToString(), ModelState);

                return StatusCode(StatusCodes.Status400BadRequest, response);
            }

            Result<User> result = await _userService.Create(userDTO);

            if (!result.IsSuccess)
            {
                int statusCode = MapError.MapErrorToStatusCode(result.ErrorCode);

                response.IsFailure(result.ErrorMessage, statusCode.ToString(), userDTO);
                return StatusCode(statusCode, response);
            }

            UserResponseDTO reponseUser = new UserResponseDTO(result.Data);

            response.IsSucess("User created successfully.", StatusCodes.Status201Created.ToString(), reponseUser);

            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginDTO)
        {
            ResponseDTO response = new ResponseDTO();
            if (!ModelState.IsValid)
            {
                response.IsFailure("Invalid login data.", StatusCodes.Status400BadRequest.ToString(), ModelState);
                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            Result<string> result = await _userService.Login(loginDTO);
            if (!result.IsSuccess)
            {
                int statusCode = MapError.MapErrorToStatusCode(result.ErrorCode);
                response.IsFailure(result.ErrorMessage, statusCode.ToString(), result.Data);
                return StatusCode(statusCode, response);
            }
            LoginResponseDTO loginResponse = new LoginResponseDTO(result.Data);
            response.IsSucess("Login successful.", StatusCodes.Status200OK.ToString(), loginResponse);
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpGet("profile")]
        [Authorize] 
        public IActionResult GetProfile()
        {
            var userId = _jwtService.GetUserIdFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
            var userEmail = _jwtService.GetUserEmailFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
            var userName = _jwtService.GetUserNameFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));

            return Ok(new
            {
                UserId = userId,
                Email = userEmail,
                Name = userName
            });
        }
    }
}

