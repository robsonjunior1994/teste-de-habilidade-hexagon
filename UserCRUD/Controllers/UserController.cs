using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserCRUD.Common;
using UserCRUD.DTOs;
using UserCRUD.DTOs.Request;
using UserCRUD.DTOs.Response;
using UserCRUD.Models;
using UserCRUD.Services;
using UserCRUD.Services.Interfaces;

namespace UserCRUD.Controllers
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
                int statusCode = MapErrorToStatusCode(result.ErrorCode);

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
                int statusCode = MapErrorToStatusCode(result.ErrorCode);
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

        private int MapErrorToStatusCode(ErrorCode errorCode)
        {
            return errorCode switch
            {
                ErrorCode.USER_ALREADY_EXISTS => StatusCodes.Status409Conflict,
                ErrorCode.INVALID_EMAIL => StatusCodes.Status422UnprocessableEntity,
                ErrorCode.USER_NOT_FOUND => StatusCodes.Status404NotFound,
                ErrorCode.INVALID_CREDENTIALS => StatusCodes.Status401Unauthorized,
                ErrorCode.DATABASE_ERROR or ErrorCode.EXTERNAL_SERVICE_UNAVAILABLE => StatusCodes.Status503ServiceUnavailable,
                _ => StatusCodes.Status400BadRequest // padrao para outros erros
            };
        }
    }
}

