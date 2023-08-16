using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class SubPerson
    {
        public long SubPersonId { get; set; }
        public int PersonId { get; set; }
        [Required, MinLength(5)]
        public string FirstName { get; set; }
        [Required, MinLength(5)]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public SubPerson(int personId, string firstName, string lastName, string email)
        {
            PersonId = personId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
        public SubPerson()
        {

        }
    }
}
