using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementBusinessLayer.ViewModels.TrainerVM;

public class DetailsVM
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string? PlanName { get; set; }
    public string DateOfBirth { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Speciality { get; set; } = null!;
}
