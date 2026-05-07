using System;
using System.Collections.Generic;

namespace BeritaDlanggu.Models;

public partial class NavSettings
{
    public int Id { get; set; }

    public int? CatId { get; set; }

    public int? Title { get; set; }

    public int? ArticleId { get; set; }

    public virtual Articles? Article { get; set; }

    public virtual Categories? Cat { get; set; }
}
