using System;
using System.Collections.Generic;

namespace BeritaDlanggu.Models;

public partial class ActivityLogs
{
    public int Id { get; set; }

    public string Action { get; set; } = null!;

    public string? Details { get; set; }

    public int UserId { get; set; }

    public string? IpAddress { get; set; }

    public DateTime Timestamp { get; set; }

    public virtual Users User { get; set; } = null!;
}
