using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementBusinessLayer.ViewModels.PlanVM;

public class PlanVM
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public int DurationDays { get; set; }
    public bool IsActive { get; set; }
}
