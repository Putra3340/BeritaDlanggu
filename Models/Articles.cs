using System;
using System.Collections.Generic;

namespace BeritaDlanggu.Models;

public partial class Articles
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string? Excerpt { get; set; }

    public string? ThumbnailUrl { get; set; }

    public int Status { get; set; }

    public bool IsFeatured { get; set; }

    public int Views { get; set; }

    public string? MetaTitle { get; set; }

    public string? MetaDescription { get; set; }

    public int AuthorId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? PublishedAt { get; set; }

    public virtual Users Author { get; set; } = null!;

    public virtual ICollection<Comments> Comments { get; set; } = new List<Comments>();

    public virtual ICollection<Categories> Category { get; set; } = new List<Categories>();
}
