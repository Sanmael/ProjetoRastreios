using System.ComponentModel.DataAnnotations;

namespace ProjetoJqueryEstudos.Models
{
    public class SubPersonModel
    {       
        public long PersonId { get; set; }
        public string FirstName { get; set; }
        [Required, MinLength(5)]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string TaxNumber { get; set; }
        public SubPersonModel()
        {                
        }
        public SubPersonModel(long personId, string firstName, string lastName, string email, string taxNumber)
        {
            PersonId = personId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            TaxNumber = taxNumber;
        }
    }
}
