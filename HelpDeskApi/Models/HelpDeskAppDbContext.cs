using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskApi;

public partial class HelpDeskAppDbContext : DbContext
{
    public HelpDeskAppDbContext()
    {
    }

    public HelpDeskAppDbContext(DbContextOptions<HelpDeskAppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserFavorite> UserFavorites { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=HelpDeskApp;User Id=sa;Password=Pa$$wor4d; encrypt=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tickets__3214EC0703255A8D");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Active).HasMaxLength(50);
            entity.Property(e => e.DateSubmitted).HasMaxLength(50);
            entity.Property(e => e.Details).HasMaxLength(50);
            entity.Property(e => e.IsBookmarked).HasMaxLength(50);
            entity.Property(e => e.Priority).HasMaxLength(50);
            entity.Property(e => e.ResolutionNote).HasMaxLength(50);
            entity.Property(e => e.ResolvedBy).HasMaxLength(50);
            entity.Property(e => e.SubmittedBy).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.User).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Tickets__UserId__398D8EEE");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC07566FC433");

            entity.ToTable("User");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<UserFavorite>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserFavo__3214EC07E7B4C2F8");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Ticket).WithMany(p => p.UserFavorites)
                .HasForeignKey(d => d.TicketId)
                .HasConstraintName("FK__UserFavor__Ticke__3D5E1FD2");

            entity.HasOne(d => d.User).WithMany(p => p.UserFavorites)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserFavor__UserI__3C69FB99");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
