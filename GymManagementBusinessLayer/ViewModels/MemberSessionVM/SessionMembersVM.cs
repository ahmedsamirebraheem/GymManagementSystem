using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementBusinessLayer.ViewModels.MemberSessionVM;

public class SessionMembersVM
{
    public int SessionId { get; set; }
    public string CategoryName { get; set; } = null!;

    public DateTime StartDate { get; set; }
    public IEnumerable<MemberBookingVM> Members { get; set; } = [];
}
