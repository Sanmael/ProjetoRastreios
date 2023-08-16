using System.ComponentModel.DataAnnotations;

namespace Service.Models
{
    public class SubPersonModel
    {
        public long SubPersonId { get; set; }
        public int PersonId { get; set; }
        [Required, MinLength(5)]
        public string Email { get; set; }
        public string FirstName { get; set; }
        [Required, MinLength(5)]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string TaxNumber { get; set; }
        public SubPersonModel()
        {                
        }
        public SubPersonModel(int personId, string firstName, string lastName, string email, string taxNumber)
        {
            PersonId = personId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            TaxNumber = taxNumber;
        }
    }
}
