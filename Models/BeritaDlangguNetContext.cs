using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BeritaDlanggu.Models;

public partial class BeritaDlangguNetContext : DbContext
{
    public BeritaDlangguNetContext()
    {
    }

    public BeritaDlangguNetContext(DbContextOptions<BeritaDlangguNetContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActivityLogs> ActivityLogs { get; set; }

    public virtual DbSet<ArticleCategories> ArticleCategories { get; set; }

    public virtual DbSet<Articles> Articles { get; set; }

    public virtual DbSet<Categories> Categories { get; set; }

    public virtual DbSet<NavSettings> NavSettings { get; set; }

    public virtual DbSet<Settings> Settings { get; set; }

    public virtual DbSet<SubCategories> SubCategories { get; set; }

    public virtual DbSet<Users> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=WIN-BNOFJBSA8BF;Initial Catalog=BeritaDlangguNET;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActivityLogs>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_ActivityLogs_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.ActivityLogs).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<ArticleCategories>(entity =>
        {
            entity.HasKey(e => new { e.ArticleId, e.CategoryId });

            entity.HasIndex(e => e.CategoryId, "IX_ArticleCategories_CategoryId");
        });

        modelBuilder.Entity<Articles>(entity =>
        {
            entity.HasIndex(e => e.AuthorId, "IX_Articles_AuthorId");

            entity.HasIndex(e => e.Slug, "IX_Articles_Slug").IsUnique();

            entity.HasOne(d => d.Author).WithMany(p => p.Articles).HasForeignKey(d => d.AuthorId);

            entity.HasOne(d => d.Cat).WithMany(p => p.Articles)
                .HasForeignKey(d => d.CatId)
                .HasConstraintName("FK_Articles_Categories");

            entity.HasOne(d => d.SubCat).WithMany(p => p.Articles)
                .HasForeignKey(d => d.SubCatId)
                .HasConstraintName("FK_Articles_SubCategories");
        });

        modelBuilder.Entity<Categories>(entity =>
        {
            entity.HasIndex(e => e.Slug, "IX_Categories_Slug").IsUnique();
        });

        modelBuilder.Entity<NavSettings>(entity =>
        {
            entity.HasOne(d => d.Article).WithMany(p => p.NavSettings)
                .HasForeignKey(d => d.ArticleId)
                .HasConstraintName("FK_NavSettings_Articles");

            entity.HasOne(d => d.Cat).WithMany(p => p.NavSettings)
                .HasForeignKey(d => d.CatId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_NavSettings_Categories");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_NavSettings_NavSettings");
        });

        modelBuilder.Entity<Settings>(entity =>
        {
            entity.HasIndex(e => e.Key, "IX_Settings_Key").IsUnique();
        });

        modelBuilder.Entity<SubCategories>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Slug).HasMaxLength(450);

            entity.HasOne(d => d.Parent).WithMany(p => p.SubCategories)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_SubCategories_Categories");
        });

        modelBuilder.Entity<Users>(entity =>
        {
            entity.HasIndex(e => e.Email, "IX_Users_Email").IsUnique();

            entity.HasIndex(e => e.Username, "IX_Users_Username").IsUnique();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
