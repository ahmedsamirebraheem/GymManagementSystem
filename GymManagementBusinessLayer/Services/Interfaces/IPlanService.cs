using GymManagementBusinessLayer.ViewModels.PlanVM;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementBusinessLayer.Services.Interfaces;

public interface IPlanService
{
    Task<IEnumerable<PlanVM>> GetAllAsync();
    Task<PlanVM?> GetAsync(int id);
    Task<UpdateVM?> GetUpdateAsync(int id);
    Task<bool> UpdateAsync(int id, UpdateVM updateVM);
    Task<bool> TogglStatus(int id);

}
