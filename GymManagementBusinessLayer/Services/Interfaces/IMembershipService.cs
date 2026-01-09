using GymManagementBusinessLayer.ViewModels.MembershipVM;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementBusinessLayer.Services.Interfaces;

public interface IMembershipService
{
    Task<IEnumerable<MembershipVM>> GetAllAsync();
    Task <MembershipVM?> GetAsync(int id);
    Task<bool> CreateAsync(CreateVM createVM);
    Task<bool> DeleteAsync(int id);
}
