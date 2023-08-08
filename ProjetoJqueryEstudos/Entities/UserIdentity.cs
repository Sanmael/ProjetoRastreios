using Microsoft.AspNetCore.Identity;

namespace ProjetoJqueryEstudos.Entities
{
    public class UserIdentity : IdentityUser
    {
        public long? PersonId { get; set; }
    }
}
