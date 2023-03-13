using Blog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data
{
    public class AppDbContext :IdentityDbContext<BlogUser,IdentityRole, string,
        IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, IdentityRoleClaim<string>, BlogUserTokens>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<BlogPost>()
                .HasOne(p => p.User)
                .WithMany(x => x.Posts)
                .HasForeignKey(x => x.UserId);

           // builder.Entity<IdentityUserToken<string>>
                
            base.OnModelCreating(builder);
        }

        public DbSet<BlogPost> BlogPost { get; set; }
    }
}
