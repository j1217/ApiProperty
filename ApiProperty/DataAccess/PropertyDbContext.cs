using ApiProperty.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ApiProperty.DataAccess
{
    public class PropertyDbContext : DbContext
    {
        public PropertyDbContext(DbContextOptions<PropertyDbContext> options) : base(options) { }

        public DbSet<Owner> Owners { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyTrace> PropertyTraces { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
    }
}
