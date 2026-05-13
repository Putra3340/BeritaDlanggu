using System;
using System.Collections.Generic;

namespace BeritaDlanggu.Models;

public partial class NavSettings
{
    public int Id { get; set; }

    public int? CatId { get; set; }

    public int? ParentId { get; set; }

    public string Title { get; set; } = null!;

    public int? ArticleId { get; set; }

    public int? SortOrder { get; set; }

    public virtual Articles? Article { get; set; }

    public virtual Categories? Cat { get; set; }

    public virtual ICollection<NavSettings> InverseParent { get; set; } = new List<NavSettings>();

    public virtual NavSettings? Parent { get; set; }
}
