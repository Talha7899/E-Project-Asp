using System;
using System.Collections.Generic;

namespace Health_Insurance_Management_System.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string Name { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Department { get; set; }

    public int? PolicyId { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<Claim> Claims { get; set; } = new List<Claim>();

    public virtual Policy? Policy { get; set; }

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual User? User { get; set; }
}
