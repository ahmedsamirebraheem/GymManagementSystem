using GymManagementBusinessLayer.ViewModels.AnalyticsVM;
using GymManagementDataAccessLayer.Entities;
using GymManagementDataAccessLayer.Repositories.Classes;
using GymManagementDataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GymManagementBusinessLayer.Services.Classes;

public class AnalyticsService(IUnitOfWork unitOfWork) : IAnalyticsService
{
    public async Task<AnalyticsVM> GetAnalyticsDataAsync()
    {
        var now = DateTime.Now;

        var membersCount = await unitOfWork.Members.CountAsync();
        var activeMembersCount = await unitOfWork.Memberships.CountAsync(m => m.EndDate > now);
        var trainersCount = await unitOfWork.Trainers.CountAsync();

       
        var upcomingCount = await unitOfWork.Sessions.CountAsync(s => s.StartDate > now);

      
        var ongoingCount = await unitOfWork.Sessions.CountAsync(s => s.StartDate <= now && s.EndDate >= now);

        
        var completedCount = await unitOfWork.Sessions.CountAsync(s => s.EndDate < now);

        return new AnalyticsVM
        {
            Members = membersCount,
            ActiveMembers = activeMembersCount,
            Trainers = trainersCount,
            UpcomingSessions = upcomingCount,
            OngoingSessions = ongoingCount,
            CompletedSessions = completedCount
        };
    }
}
