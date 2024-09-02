using System;
using System.Collections.Generic;

namespace Health_Insurance_Management_System.Models;

public partial class Request
{
    public int RequestId { get; set; }

    public int? EmployeeId { get; set; }

    public string? RequestType { get; set; }

    public DateTime RequestDate { get; set; }

    public int? InsuranceCompanyId { get; set; }

    public int? PolicyId { get; set; }

    public string Status { get; set; } = null!;

    public virtual Employee? Employee { get; set; }

    public virtual InsuranceCompany? InsuranceCompany { get; set; }

    public virtual Policy? Policy { get; set; }
}
