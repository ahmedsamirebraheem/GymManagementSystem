using GymManagementBusinessLayer.Services.Interfaces;
using GymManagementBusinessLayer.ViewModels.MembershipVM;
using GymManagementDataAccessLayer.Entities;
using GymManagementDataAccessLayer.Repositories.Interfaces;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementBusinessLayer.Services.Classes;

public class MembershipService(IUnitOfWork unitOfWork, IMapper mapper) : IMembershipService
{
    public async Task<bool> CreateAsync(CreateVM createVM)
    {
        try
        {
            var plan = await unitOfWork.Plans.GetAsync(p => p.Id == createVM.PlanId);
            if (plan == null) return false;

            var membership = new Membership
            {
                MemberId = createVM.MemberId,
                PlanId = createVM.PlanId,
                EndDate = DateTime.Now.AddDays(plan.DurationDays),
                CreatedAt = DateTime.Now
            };
            await unitOfWork.Memberships.AddAsync(membership);
            await unitOfWork.SaveChangesAsync();
            return true;

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return false;
        }
    }

    public async Task<IEnumerable<MembershipVM>> GetAllAsync()
    {
        var memberships = await unitOfWork.Memberships.GetAllAsync(
            filter: m => !m.IsDeleted,
            m => m.Member,
            m => m.Plan
        );
        return mapper.Map<IEnumerable<MembershipVM>>(memberships);
    }

    public async Task<MembershipVM?> GetAsync(int id)
    {
        var membership = await unitOfWork.Memberships.GetAsync(
            filter: m => m.Id == id && !m.IsDeleted,
            includeProperties: "Member,Plan"
        );
        if (membership == null) return null;
        return mapper.Map<MembershipVM>(membership);

    }
    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var membership = await unitOfWork.Memberships.GetAsync(m => m.Id == id);

            if (membership == null) return false;

          
            unitOfWork.Memberships.Delete(membership);

            await unitOfWork.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during cancellation: {ex.Message}");
            return false;
        }
    }
}
