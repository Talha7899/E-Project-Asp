using System;
using System.Collections.Generic;

namespace Health_Insurance_Management_System.Models;

public partial class Report
{
    public int ReportId { get; set; }

    public string? ReportType { get; set; }

    public DateTime? ReportDate { get; set; }

    public int? GeneratedBy { get; set; }

    public string? Details { get; set; }

    public virtual User? GeneratedByNavigation { get; set; }
}
