
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PA.Core.Models;

namespace PA.Datastore.EFCore
{
	public class ApplicationManagmentDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationManagmentDbContext(DbContextOptions<ApplicationManagmentDbContext> options) : base(options)
        { }


        public DbSet<Vendor> PetrolVendors { get; set; }
        public DbSet<Station> PetrolStations { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<StationRating> StationRatings { get; set; }

        public DbSet<CustomerHistory> StationCustomerHistory { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //  Add constraint unique vendor code
            modelBuilder.Entity<Vendor>()
                .HasIndex(c => c.VendorCode)
                .IsUnique();

        }


    }
}
