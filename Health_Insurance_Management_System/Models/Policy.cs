using System;
using System.Collections.Generic;

namespace Health_Insurance_Management_System.Models;

public partial class Policy
{
    public int PolicyId { get; set; }

    public int? InsuranceCompanyId { get; set; }

    public string PolicyName { get; set; } = null!;

    public string? CoverageDetails { get; set; }

    public virtual ICollection<Claim> Claims { get; set; } = new List<Claim>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual InsuranceCompany? InsuranceCompany { get; set; }

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
