using DoctorSchedule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorSchedule.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Attendee> Attendees { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.StartTime).IsRequired();
                entity.Property(e => e.EndTime).IsRequired();
            });

            modelBuilder.Entity<Attendee>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Name).HasMaxLength(50).IsRequired();
                entity.Property(a => a.Email).HasMaxLength(100).IsRequired();
                entity.HasOne(a => a.Event)
                      .WithMany(e => e.Attendees)
                      .HasForeignKey(a => a.EventId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
