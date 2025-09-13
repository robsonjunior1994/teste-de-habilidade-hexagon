using System.ComponentModel.DataAnnotations;

namespace Hexagon.Api.Presentation.DTOs.Request
{
    public class CustomerRequestDTO
    {
        [Required(ErrorMessage = "The Name field is required.")]
        public string Name { get; set; }
        public string Age { get; set; }
        public string CivilState { get; set; }
        [Required(ErrorMessage = "The Cpf field is required.")]
        public string Cpf { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
