using System;
using System.Collections.Generic;

namespace BeritaDlanggu.Models;

public partial class Categories
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string? Description { get; set; }

    public int? ParentId { get; set; }

    public int SortOrder { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Categories> InverseParent { get; set; } = new List<Categories>();

    public virtual Categories? Parent { get; set; }

    public virtual ICollection<Articles> Article { get; set; } = new List<Articles>();
}
