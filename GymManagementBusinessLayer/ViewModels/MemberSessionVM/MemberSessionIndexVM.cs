using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementBusinessLayer.ViewModels.MemberSessionVM;

public class MemberSessionIndexVM
{
    public IEnumerable<MemberSessionVM> UpcomingSessions { get; set; } = [];
    public IEnumerable<MemberSessionVM> OngoingSessions { get; set; } = [];
}
