using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Lunch_App.Models
{
    public class LunchUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<LunchUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public string Handle { get; set; }
        public virtual List<LunchUser> MyBuddies { get; set; }
        public virtual List<LunchUser> BuddiesWithMe { get; set; }
        public virtual List<Resturant> FavoriteResturants { get; set; }
        public virtual List<LunchMembers> Lunches { get; set; }

    }

    public class ApplicationDbContext : IdentityDbContext<LunchUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<LunchUser>().ToTable("Users").HasMany(x => x.MyBuddies).WithMany(x => x.BuddiesWithMe)
               .Map(x =>
            {
                x.ToTable("LunchBuddies");
                x.MapLeftKey("LunchUserId");
                x.MapRightKey("BuddyId");
            });
            modelBuilder.Entity<Resturant>().HasMany(x => x.Fans).WithMany(y => y.FavoriteResturants)
                .Map(z => z.ToTable("ResturantFans"));
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
        }
    }
}