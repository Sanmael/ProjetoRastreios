using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Person
    {
        public int PersonId { get; set; }
        [Required, MinLength(5)]
        public string FirstName { get; set; }
        [Required, MinLength(5)]
        public string LastName { get; set; }
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$")]
        [Required, MinLength(14), MaxLength(14)]
        public string TaxNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public Person()
        {
                
        }

        public Person(string firstName, string lastName, string taxNumber, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            TaxNumber = taxNumber;
            Email = email;
        }
    }
}

