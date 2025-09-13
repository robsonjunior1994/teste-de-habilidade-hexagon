using Hexagon.Api.Domain.Models;
using Hexagon.Api.Domain.Services.Interfaces;
using Hexagon.Api.Presentation.Common;
using Hexagon.Api.Presentation.DTOs;
using Hexagon.Api.Presentation.DTOs.Request;
using Hexagon.Api.Presentation.DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hexagon.Api.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
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

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            ResponseDTO response = new ResponseDTO();

            // Validação dos parâmetros
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Result<PaginatedResult<Customer>> result = await _customerService.GetAll(int.Parse(userId), pageNumber, pageSize);

            if (!result.IsSuccess)
            {
                int statusCode = MapError.MapErrorToStatusCode(result.ErrorCode);
                response.IsFailure(result.ErrorMessage, statusCode.ToString(), result.Data);
                return StatusCode(statusCode, response);
            }

            var paginatedResponse = new
            {
                Items = result.Data.Items.Select(c => new CustomerResponseDTO(c)).ToList(),
                result.Data.TotalCount,
                result.Data.PageNumber,
                result.Data.PageSize,
                result.Data.TotalPages,
                result.Data.HasPreviousPage,
                result.Data.HasNextPage
            };

            response.IsSucess("Customers retrieved successfully.", StatusCodes.Status200OK.ToString(), paginatedResponse);
            return Ok(response);
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetForId([FromRoute]int customerId)
        {
            ResponseDTO response = new ResponseDTO();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Result<Customer> result = await _customerService.GetForId(int.Parse(userId), customerId);

            if (!result.IsSuccess)
            {
                int statusCode = MapError.MapErrorToStatusCode(result.ErrorCode);
                response.IsFailure(result.ErrorMessage, statusCode.ToString(), result.Data);
                return StatusCode(statusCode, response);
            }

            CustomerResponseDTO reponseCustomer = new CustomerResponseDTO(result.Data);
            response.IsSucess("Customer retrieved successfully.", StatusCodes.Status200OK.ToString(), reponseCustomer);
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpDelete("{customerId}")]
        public async Task<IActionResult> Delete([FromRoute]int customerId)
        {
            ResponseDTO response = new ResponseDTO();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Result<Customer> result = await _customerService.DeleteForId(int.Parse(userId), customerId);

            if (!result.IsSuccess)
            {
                int statusCode = MapError.MapErrorToStatusCode(result.ErrorCode);
                response.IsFailure(result.ErrorMessage, statusCode.ToString(), result.Data);
                return StatusCode(statusCode, response);
            }
            CustomerResponseDTO reponseCustomer = new CustomerResponseDTO(result.Data);
            response.IsSucess("Customer deleted successfully.", StatusCodes.Status200OK.ToString(), reponseCustomer);
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPut("{customerId}")]
        public async Task<IActionResult> Put([FromRoute] int customerId, [FromBody] CustomerRequestDTO customerForUpdate)
        {
            ResponseDTO response = new ResponseDTO();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Result<Customer> result = await _customerService.Update(int.Parse(userId), customerId, customerForUpdate);

            if (!result.IsSuccess)
            {
                int statusCode = MapError.MapErrorToStatusCode(result.ErrorCode);
                response.IsFailure(result.ErrorMessage, statusCode.ToString(), result.Data);
                return StatusCode(statusCode, response);
            }
            CustomerResponseDTO reponseCustomer = new CustomerResponseDTO(result.Data);
            response.IsSucess("Customer update successfully.", StatusCodes.Status200OK.ToString(), reponseCustomer);
            return StatusCode(StatusCodes.Status200OK, response);
        }
    }
}
