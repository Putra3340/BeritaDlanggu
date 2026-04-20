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

    public virtual DbSet<Articles> Articles { get; set; }

    public virtual DbSet<Categories> Categories { get; set; }

    public virtual DbSet<Comments> Comments { get; set; }

    public virtual DbSet<Settings> Settings { get; set; }

    public virtual DbSet<Users> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=WIN-BNOFJBSA8BF;Initial Catalog=BeritaDlangguNET;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActivityLogs>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_ActivityLogs_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.ActivityLogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Articles>(entity =>
        {
            entity.HasIndex(e => e.AuthorId, "IX_Articles_AuthorId");

            entity.HasIndex(e => e.Slug, "IX_Articles_Slug").IsUnique();

            entity.HasOne(d => d.Author).WithMany(p => p.Articles)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasMany(d => d.Category).WithMany(p => p.Article)
                .UsingEntity<Dictionary<string, object>>(
                    "ArticleCategories",
                    r => r.HasOne<Categories>().WithMany().HasForeignKey("CategoryId"),
                    l => l.HasOne<Articles>().WithMany().HasForeignKey("ArticleId"),
                    j =>
                    {
                        j.HasKey("ArticleId", "CategoryId");
                        j.HasIndex(new[] { "CategoryId" }, "IX_ArticleCategories_CategoryId");
                    });
        });

        modelBuilder.Entity<Categories>(entity =>
        {
            entity.HasIndex(e => e.ParentId, "IX_Categories_ParentId");

            entity.HasIndex(e => e.Slug, "IX_Categories_Slug").IsUnique();

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent).HasForeignKey(d => d.ParentId);
        });

        modelBuilder.Entity<Comments>(entity =>
        {
            entity.HasIndex(e => e.ArticleId, "IX_Comments_ArticleId");

            entity.HasOne(d => d.Article).WithMany(p => p.Comments).HasForeignKey(d => d.ArticleId);
        });

        modelBuilder.Entity<Settings>(entity =>
        {
            entity.HasIndex(e => e.Key, "IX_Settings_Key").IsUnique();
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
