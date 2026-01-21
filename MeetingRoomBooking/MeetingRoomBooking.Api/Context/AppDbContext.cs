using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MeetingRoomBooking.Api.Models;

namespace MeetingRoomBooking.Api.Context
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<booking> bookings { get; set; } = null!;
        public virtual DbSet<room> rooms { get; set; } = null!;
        public virtual DbSet<user> users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=meeting_booking;Username=postgres;Password=Poplol33_");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<booking>(entity =>
            {
                entity.Property(e => e.created_at)
                    .HasColumnType("timestamp without time zone")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.end_time).HasColumnType("timestamp without time zone");

                entity.Property(e => e.start_time).HasColumnType("timestamp without time zone");

                entity.Property(e => e.title).HasMaxLength(200);

                entity.HasOne(d => d.room)
                    .WithMany(p => p.bookings)
                    .HasForeignKey(d => d.room_id)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("bookings_room_id_fkey");

                entity.HasOne(d => d.user)
                    .WithMany(p => p.bookings)
                    .HasForeignKey(d => d.user_id)
                    .HasConstraintName("bookings_user_id_fkey");
            });

            modelBuilder.Entity<room>(entity =>
            {
                entity.HasIndex(e => e.name, "rooms_name_key")
                    .IsUnique();

                entity.Property(e => e.name).HasMaxLength(100);
            });

            modelBuilder.Entity<user>(entity =>
            {
                entity.HasIndex(e => e.username, "users_username_key")
                    .IsUnique();

                entity.Property(e => e.created_at)
                    .HasColumnType("timestamp without time zone")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.password).HasMaxLength(200);

                entity.Property(e => e.role).HasMaxLength(10);

                entity.Property(e => e.username).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
