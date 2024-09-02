using System;
using System.Collections.Generic;

namespace Health_Insurance_Management_System.Models;

public partial class Claim
{
    public int ClaimId { get; set; }

    public int? PolicyId { get; set; }

    public int? EmployeeId { get; set; }

    public decimal? ClaimAmount { get; set; }

    public string? ClaimStatus { get; set; }

    public DateTime? ClaimDate { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Policy? Policy { get; set; }
}
