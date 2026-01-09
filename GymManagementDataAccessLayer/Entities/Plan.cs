using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementDataAccessLayer.Entities;

public class Plan : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public int DurationDays { get; set; }
    public bool IsActive { get; set; }

    public ICollection<Membership> Memberships { get; set; } = [];
}
