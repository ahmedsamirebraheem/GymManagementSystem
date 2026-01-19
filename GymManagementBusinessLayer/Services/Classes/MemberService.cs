using GymManagementDataAccessLayer.Entities; 
using GymManagementDataAccessLayer.Repositories.Interfaces; 
using MapsterMapper;
using GymManagementBusinessLayer.Services.Interfaces;
using GymManagementBusinessLayer.ViewModels.MemberVM;
using GymManagementBusinessLayer.Services.Interfaces.AttachmentService;
namespace GymManagementBusinessLayer.Services.Classes;

public class MemberService(IUnitOfWork unitOfWork, IMapper mapper, IAttachmentService attachmentService) : IMemberService
{
    public async Task<bool> CreateAsync(CreateVM createVM)
    {
        try
        {
            var emailOrPhoneExist = await unitOfWork.Members.GetAsync(m => m.Email == createVM.Email || m.PhoneNumber == createVM.PhoneNumber);
            if (emailOrPhoneExist != null) return false;

            var member = mapper.Map<Member>(createVM);

            // رفع الصورة وحفظ المسار في الـ Entity
            if (createVM.PhotoFile != null)
            {
                member.Photo = await attachmentService.UploadAsync("members", createVM.PhotoFile);
            }

            await unitOfWork.Members.AddAsync(member);
            await unitOfWork.SaveChangesAsync();
            return true;
        }
        catch (Exception) { return false; }
    }

    public async Task<bool> UpdateAsync(int id, UpdateVM updateVM)
    {
        try
        {
            var member = await unitOfWork.Members.GetAsync(m => m.Id == id);
            if (member == null) return false;

            // لو رفع صورة جديدة
            if (updateVM.PhotoFile != null)
            {
                // 1. حذف القديمة
                if (!string.IsNullOrEmpty(member.Photo))
                {
                    var oldFileName = Path.GetFileName(member.Photo);
                    await attachmentService.DeleteAsync("members", oldFileName);
                }
                // 2. رفع الجديدة
                member.Photo = await attachmentService.UploadAsync("members", updateVM.PhotoFile);
            }

            // عمل الـ Map لباقي البيانات
            mapper.Map(updateVM, member);

            unitOfWork.Members.Update(member);
            await unitOfWork.SaveChangesAsync();
            return true;
        }
        catch (Exception) { return false; }
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
           var isDeleted =  await unitOfWork.SaveChangesAsync();
            if(isDeleted != 0 )
            {
                await attachmentService.DeleteAsync(member.Photo, "members");
                return true;
            }
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }

    
}