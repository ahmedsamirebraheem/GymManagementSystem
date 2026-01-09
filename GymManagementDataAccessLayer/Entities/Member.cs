using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementDataAccessLayer.Entities;

public class Member : GymUser
{
    public string? Photo { get; set; }

    public HealthRecord HealthRecord { get; set; } = null!;

    public ICollection<Membership> Memberships { get; set; } = [];

    public ICollection<MemberSession> MemberSessions { get; set; } = [];
}
