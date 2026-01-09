using GymManagementDataAccessLayer.Entities; 
using GymManagementDataAccessLayer.Repositories.Interfaces; 
using MapsterMapper;
using GymManagementBusinessLayer.Services.Interfaces;
using GymManagementBusinessLayer.ViewModels.MemberVM;
namespace GymManagementBusinessLayer.Services.Classes;

public class MemberService(IUnitOfWork unitOfWork, IMapper mapper) : IMemberService
{
    public async Task<bool> CreateAsync(CreateVM createVM)
    {
        try
        {
            var emailOrPhoneExist = await unitOfWork.Members.GetAsync(m => m.Email == createVM.Email || m.PhoneNumber == createVM.PhoneNumber);

            if (emailOrPhoneExist != null) return false;

            var member = mapper.Map<Member>(createVM);
            await unitOfWork.Members.AddAsync(member);
            await unitOfWork.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }

    }
    public async Task<IEnumerable<MemberVM>> GetAllAsync()
    {
        var members = await unitOfWork.Members.GetAllAsync();
        if (members == null || !members.Any())
        {
            return [];
        }

        return mapper.Map<IEnumerable<MemberVM>>(members);
    }
    public async Task<DetailsVM?> GetDetailsAsync(int id)
    {
        var member = await unitOfWork.Members.GetAsync(
            filter: x => x.Id == id,
            includeProperties: "Memberships.Plan" // تأكد إن الـ Address بيتحمل تلقائياً كونه Owned Type
        );

        if (member == null) return null;

        // Mapster هنا هيحول الـ Address (Object) لـ Address (String) بناءً على الـ Config
        var details = mapper.Map<DetailsVM>(member);

        // منطق الـ Membership زي ما هو
        var activeMembership = member.Memberships
            .OrderByDescending(m => m.CreatedAt)
            .FirstOrDefault(m => m.Status == "Active");

        if (activeMembership != null)
        {
            details.PlanName = activeMembership.Plan?.Name;
            details.MembershipStartDate = activeMembership.CreatedAt.ToString("dd-MM-yyyy");
            details.MembershipEndDate = activeMembership.EndDate.ToString("dd-MM-yyyy");
        }
        else
        {
            details.PlanName = "No Active Plans";
        }

        return details;
    }
    public async Task<HealthRecordVM?> GetHealthRecordAsync(int id)
    {
        var member = await unitOfWork.Members.GetAsync(
        filter: x => x.Id == id,
        includeProperties: "HealthRecord");
        if (member == null || member.HealthRecord == null)
        {
            return null;
        }
        var healthRecordVM = mapper.Map<HealthRecordVM>(member.HealthRecord);
        return healthRecordVM;
    }
    public async Task<UpdateVM?> GetUpdateAsync(int id)
    {
        var member = await unitOfWork.Members.GetAsync(m => m.Id == id);
        if (member == null) return null;
        var updateVM = mapper.Map<UpdateVM>(member);
        return updateVM;
    }
    public async Task<bool> UpdateAsync(int id, UpdateVM updateVM)
    {
        try
        {
            var emailOrPhoneExistForAnotherMember = await unitOfWork.Members.GetAsync(m =>
                m.Id != id && (m.Email == updateVM.Email || m.PhoneNumber == updateVM.PhoneNumber));

            if (emailOrPhoneExistForAnotherMember != null) return false;

            var member = await unitOfWork.Members.GetAsync(m => m.Id == id);
            if (member == null) return false;
            mapper.Map(updateVM, member);

            unitOfWork.Members.Update(member);
            await unitOfWork.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
           
            var futureSessionExist = await unitOfWork.MemberSessions.GetAsync(
                filter: x => x.MemberId == id && x.Session.EndDate > DateTime.Now,
                includeProperties: "Session"
            );

            if (futureSessionExist != null) return false;
            var memberships = await unitOfWork.Memberships.GetAllAsync(
                filter: x => x.MemberId == id 
            );
            foreach (var membership in memberships)
            {
                unitOfWork.Memberships.Delete(membership);
            }

            var member = await unitOfWork.Members.GetAsync(m => m.Id == id);
            if (member == null) return false;

            unitOfWork.Members.Delete(member);
            await unitOfWork.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    
}