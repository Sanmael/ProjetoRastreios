using System.ComponentModel.DataAnnotations;

namespace ProjetoJqueryEstudos.Models
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        //[CustomAnnotation("O campo {0} deve conter letras e números, com no mínimo 10 caracteres.")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "As senhas não correspondem")]
        public string ConfirmPassword { get; set; }        
        [Required, MinLength(5)]
        public string FirstName { get; set; }
        [Required, MinLength(5)]
        public string LastName { get; set; }
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$")]
        [Required, MinLength(14), MaxLength(14)]
        public string TaxNumber { get; set; }
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required, MinLength(8), MaxLength(8)]
        public string PostalCode { get; set; }
        [Required]
        public string Neighborhood { get; set; }
        [Required]
        public string PublicPlace { get; set; }
        public bool isPrincipalAddress { get; set; }
    }
}
