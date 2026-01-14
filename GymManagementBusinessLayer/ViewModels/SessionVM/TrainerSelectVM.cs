using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementBusinessLayer.ViewModels.SessionVM;

public class TrainerSelectVM
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int SpecializationId { get; set; }
}
