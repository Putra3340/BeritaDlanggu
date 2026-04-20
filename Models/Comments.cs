using System;
using System.Collections.Generic;

namespace BeritaDlanggu.Models;

public partial class Comments
{
    public int Id { get; set; }

    public int ArticleId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Message { get; set; } = null!;

    public bool IsApproved { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Articles Article { get; set; } = null!;
}
