using GymManagementBusinessLayer.Services.Classes;
using GymManagementBusinessLayer.ViewModels.MemberSessionVM;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementBusinessLayer.Services.Interfaces;

public interface IMemberSessionService
{
    Task<MemberSessionIndexVM> GetAllAsync();
    Task<SessionMembersVM> GetAsync(int sessionId);
    Task<bool> CancelBookingAsync(int sessionId, int memberId);
    Task<CreateMemberSessionVM> PrepareCreateViewModelAsync(int sessionId);
    Task<bool> CreateAsync(CreateMemberSessionVM model);
    Task<bool> ToggleAttendanceAsync(int sessionId, int memberId);
    
}
