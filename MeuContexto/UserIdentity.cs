using Microsoft.AspNetCore.Identity;

namespace MeuContexto
{
    public class UserIdentity : IdentityUser
    {
        public int? PersonId { get; set; }
    }
}
