using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementBusinessLayer.ViewModels.AnalyticsVM;

public class AnalyticsVM
{
    public int Members { get; set; }
    public int ActiveMembers { get; set; }
    public int Trainers { get; set; }
    public int UpcomingSessions { get; set; }
    public int OngoingSessions { get; set; }
    public int CompletedSessions { get; set; }

}
