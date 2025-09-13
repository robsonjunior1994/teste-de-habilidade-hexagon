using Hexagon.Api.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Hexagon.Api.Presentation.DTOs.Response
{
    public class CustomerResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public string CivilState { get; set; }
        public string Cpf { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public CustomerResponseDTO()
        {
            
        }

        public CustomerResponseDTO(Customer customer)
        {
            Id = customer.Id;
            Name = customer.Name;
            Age = customer.Age;
            CivilState = customer.CivilState;
            Cpf = customer.Cpf;
            City = customer.City;
            State = customer.State;
        }
    }
}
