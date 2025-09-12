using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserCRUD.Common;
using UserCRUD.DTOs;
using UserCRUD.DTOs.Request;
using UserCRUD.DTOs.Response;
using UserCRUD.Models;
using UserCRUD.Services.Interfaces;

namespace UserCRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IJwtService _jwtService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerRequestDTO customerRequestDTO)
        {
            ResponseDTO response = new ResponseDTO();

            if (!ModelState.IsValid)
            {
                response.IsFailure("Invalid customer data.", StatusCodes.Status400BadRequest.ToString(), ModelState);

                return StatusCode(StatusCodes.Status400BadRequest, response);
            }

            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            Result<Customer> result = await _customerService.Create(customerRequestDTO, email);

            if (!result.IsSuccess)
            {
                int statusCode = MapError.MapErrorToStatusCode(result.ErrorCode);

                response.IsFailure(result.ErrorMessage, statusCode.ToString(), result.Data);
                return StatusCode(statusCode, response);
            }

            CustomerResponseDTO reponseCustomer = new CustomerResponseDTO(result.Data);

            response.IsSucess("Customer created successfully.", StatusCodes.Status201Created.ToString(), reponseCustomer);

            return StatusCode(StatusCodes.Status201Created, response);
        }
    }
}
