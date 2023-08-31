using Domain;
using MeuContexto;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MeuContexto.Context
{
    public class EntityContext : IdentityDbContext<UserIdentity>
    {
        public EntityContext(DbContextOptions<EntityContext> options) : base(options) { }
        public DbSet<Person> Person { get; set; }
        public DbSet<SubPerson> SubPerson { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<ErrorLog> ErrorLog { get; set; }
        public DbSet<TrackingCode> TrackingCode { get; set; }
        public DbSet<TrackingCodeEvents> TrackingCodeEvents { get; set; }
    }
}