using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SBapi.Common.Dto;
using SBapi.Entity.Models;
using SBapi.Entity.Security;

namespace SBapi.Entity.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<AccountType>().HasIndex(x => x.TypeName).IsUnique();

            // Seed the database with a SuperAdmin user and role
            AppUser user = new()
            {
                Id = "0ca18703-c5ca-41bb-a1bb-a0a7665aa8b9",
                UserName = "superadmin@gmail.com",
                NormalizedUserName = "SUPERADMIN@GMAIL.COM",
                Email = "superadmin@gmail.com",
                NormalizedEmail = "SUPERADMIN@GMAIL.COM",
                IsActive = true,
            };

            string password = "SuperAdmin@123";
            PasswordHasher<AppUser> passwordHasher = new PasswordHasher<AppUser>();
            user.PasswordHash = passwordHasher.HashPassword(user, password);
            modelBuilder.Entity<AppUser>().HasData(user);

            IdentityRole role = new()
            {
                Id = "b1c2d3e4-f5g6-h7i8-j9k0-l1m2n3o4p5q6",
                Name = "SuperAdmin",
                NormalizedName = "SUPERADMIN"
            };
            modelBuilder.Entity<IdentityRole>().HasData(role);

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                UserId = user.Id,
                RoleId = role.Id
            });

            // IFSC Sequence for Branches
            modelBuilder.Entity<SequenceValue>().HasNoKey().ToView(null);

            modelBuilder.HasSequence<int>("IFSCSequence", schema: "dbo")
                .StartsAt(005993)
                .IncrementsBy(1);
            modelBuilder.Entity<Branch>()
                .Property(b => b.IFSC)
                .HasMaxLength(50);

            // AccountNumber Sequence for Accounts
            modelBuilder.HasSequence<int>("AccountNumberSequence", schema: "dbo")
                .StartsAt(11235813)
                .IncrementsBy(1);
            modelBuilder.Entity<Account>()
                .Property(b => b.AccountNumber)
                .HasMaxLength(50);

            // ApplicationForm - Store Enum as String 
            modelBuilder.Entity<ApplicationForm>()
                .Property(f => f.Status)
                .HasConversion<string>()
                .HasMaxLength(20); 
        }


        public DbSet<Branch> BranchSet { get; set; }
        public DbSet<ApplicationForm> ApplicationFormSet { get; set; }
        public DbSet<AccountType> AccountTypeSet { get; set; }
        public DbSet<Account> AccountSet { get; set; }
        public DbSet<Transactions> TransactionsSet { get; set; }
    }
}
