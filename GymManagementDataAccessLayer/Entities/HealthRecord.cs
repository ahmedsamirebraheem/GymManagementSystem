using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementDataAccessLayer.Entities;

public class HealthRecord : BaseEntity
{
    public decimal Height { get; set; }
    public decimal Weight { get; set; }
    public string BloodType { get; set; } = null!;
    public string? Note { get; set; }
}
