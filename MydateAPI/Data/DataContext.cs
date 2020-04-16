using Microsoft.EntityFrameworkCore;
using MydateAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MydateAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Value> Values { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Like> Likes { get; set; }
        //public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //set a primary Key of Like class(LikerId, LikeeId)
            builder.Entity<Like>()
                .HasKey(k => new { k.LikerId, k.LikeeId });

            //Tell to EF about the relationship Likee 1<-->* Liker
            //Restrict DeleteBehavior so that when deleting a like a user isn't delete
            builder.Entity<Like>()
               .HasOne(u => u.Likee)
               .WithMany(u => u.Likers)
               .HasForeignKey(u => u.LikeeId)
               .OnDelete(DeleteBehavior.Restrict);

            //Tell to EF about the relationship Liker 1<-->* Likee
            builder.Entity<Like>()
               .HasOne(u => u.Liker)
               .WithMany(u => u.Likees)
               .HasForeignKey(u => u.LikerId)
               .OnDelete(DeleteBehavior.Restrict);

            /*builder.Entity<Message>()
              .HasOne(u => u.Sender)
              .WithMany(m => m.MessagesSent)
              .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
              .HasOne(u => u.Recipient)
              .WithMany(m => m.MessagesReceived)
              .OnDelete(DeleteBehavior.Restrict);*/
        }
    }
}
