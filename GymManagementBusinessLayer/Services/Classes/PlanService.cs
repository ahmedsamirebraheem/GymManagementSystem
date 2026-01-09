using GymManagementBusinessLayer.Services.Interfaces;
using GymManagementBusinessLayer.ViewModels.PlanVM;
using GymManagementDataAccessLayer.Entities;
using GymManagementDataAccessLayer.Repositories.Interfaces;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementBusinessLayer.Services.Classes;

public class PlanService(IUnitOfWork unitOfWork, IMapper mapper) : IPlanService
{
    public async Task<IEnumerable<PlanVM>> GetAllAsync()
    {
        var plans = await unitOfWork.Plans.GetAllAsync();
        if(plans == null) return [];

        return mapper.Map<IEnumerable<PlanVM>>(plans);
    }

    public async Task<PlanVM?> GetAsync(int id)
    {
        var plan = await unitOfWork.Plans.GetAsync(p => p.Id == id);
        if (plan == null)
            return null;
        return mapper.Map<PlanVM>(plan);
    }

    public async Task<UpdateVM?> GetUpdateAsync(int id)
    {
        var plan = await unitOfWork.Plans.GetAsync(p => p.Id == id);
        if (plan == null)
            return null;
        return mapper.Map<UpdateVM>(plan);
    }

    public async Task<bool> UpdateAsync(int id, UpdateVM updateVM)
    {
        var planWithActiveMembership = await unitOfWork.Memberships.GetAsync(m =>
            m.PlanId == id && m.EndDate > DateTime.Now);
        if (planWithActiveMembership != null)
            return false;

        var plan = await unitOfWork.Plans.GetAsync(p => p.Id == id);
        if (plan == null) return false;

        mapper.Map(updateVM, plan);

        unitOfWork.Plans.Update(plan);
        await unitOfWork.SaveChangesAsync();
        return true;

    }
    public async Task<bool> TogglStatus(int id)
    {
        var plan = await unitOfWork.Plans.GetAsync(p => p.Id == id);
        if (plan == null) return false;
        plan.IsActive = !plan.IsActive;
        plan.UpdatedAt = DateTime.Now;
        try
        {
            unitOfWork.Plans.Update(plan);
            await unitOfWork.SaveChangesAsync();
            return true;
        }
        catch(Exception)
        {
            return false;
        }
    }
}
