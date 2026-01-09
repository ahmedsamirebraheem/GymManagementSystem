using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementBusinessLayer.ViewModels.MemberVM;

public class DetailsVM
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Photo { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string Gender { get; set; } = null!;
    public string? PlanName { get; set; }
    public string DateOfBirth { get; set; } = null!;
    public string? MembershipStartDate { get; set; } 
    public string? MembershipEndDate { get; set; }
    public string Address { get; set; } = null!;

}
