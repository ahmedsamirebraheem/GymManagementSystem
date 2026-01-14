using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementBusinessLayer.ViewModels.TrainerVM;

public class TrainerVM
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Specialization { get; set; } = null!;
    public int SpecializationId { get; set; }
    public string Gender { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
}
