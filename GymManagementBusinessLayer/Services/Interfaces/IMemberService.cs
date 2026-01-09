using GymManagementBusinessLayer.ViewModels.MemberVM;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementBusinessLayer.Services.Interfaces;

public interface IMemberService
{
    Task<IEnumerable<MemberVM>> GetAllAsync();
    Task<bool> CreateAsync(CreateVM createVM);
    Task<DetailsVM?> GetDetailsAsync(int id);
    Task<HealthRecordVM?> GetHealthRecordAsync(int id);
    Task<UpdateVM?> GetUpdateAsync(int id);
    Task<bool> UpdateAsync(int id, UpdateVM updateVM);
    Task<bool> DeleteAsync(int id);


}
