using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementBusinessLayer.ViewModels.AnalyticsVM;

public interface IAnalyticsService
{
    Task<AnalyticsVM> GetAnalyticsDataAsync();
}
