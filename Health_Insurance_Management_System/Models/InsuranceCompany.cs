using System;
using System.Collections.Generic;

namespace Health_Insurance_Management_System.Models;

public partial class InsuranceCompany
{
    public int InsuranceCompanyId { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string ContactNumber { get; set; } = null!;

    public virtual ICollection<Policy> Policies { get; set; } = new List<Policy>();

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
