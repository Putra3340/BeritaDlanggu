using System;
using System.Collections.Generic;

namespace BeritaDlanggu.Models;

public partial class Categories
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Articles> Articles { get; set; } = new List<Articles>();

    public virtual ICollection<SubCategories> SubCategories { get; set; } = new List<SubCategories>();
}
