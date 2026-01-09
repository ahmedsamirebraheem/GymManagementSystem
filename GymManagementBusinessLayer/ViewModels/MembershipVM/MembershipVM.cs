using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementBusinessLayer.ViewModels.MembershipVM;


public class MembershipVM
{
    public int Id { get; set; }

    public string MemberName { get; set; } = null!;

    public string PlanName { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int MemberId { get; set; }
}

