using Microsoft.EntityFrameworkCore;
using Relaxinema.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Relaxinema.Infrastructure.DatabaseContext
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Film> Films { get; set; }
        public DbSet<FilmGenre> FilmGenres { get; set;}
        public DbSet<Genre> Genres { get; set; }
         public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Film>()
                .HasMany(f => f.Genres)
                .WithMany(g => g.Films)
                .UsingEntity<FilmGenre>();

            builder.Entity<Film>()
                .HasMany(f => f.SubscribedUsers)
                .WithMany(u=> u.SubscribedTo)
                .UsingEntity<Subscription>();
            
            builder.Entity<User>(b => 
                b.HasMany(e => e.Roles)
                .WithOne()
                .HasForeignKey(ur => ur.UserId)
                .IsRequired());

            // builder.Entity<User>()
            //     .HasMany(u => u.Roles)
            //     .WithMany(r => r.Users)
            //     .UsingEntity<UserRole>();
        }
    }
}
