using System;
using System.Collections.Generic;

namespace BeritaDlanggu.Models;

public partial class SubCategories
{
    public int Id { get; set; }

    public int ParentId { get; set; }

    public string Name { get; set; } = null!;

    public string? Slug { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Articles> Articles { get; set; } = new List<Articles>();

    public virtual Categories Parent { get; set; } = null!;
}
