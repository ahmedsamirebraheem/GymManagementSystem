using GymManagementBusinessLayer.ViewModels.SessionVM;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementBusinessLayer.Services.Interfaces;

public interface ISessionService
{
    Task<IEnumerable<SessionVM>> GetAllAsync();
    Task<SessionVM?> GetAsync(int sessionId);

    Task<bool> CreateAsync(CreateVM createVM);
    Task<UpdateVM?> GetUpdateAsync(int id);
    Task<bool> UpdateAsync(int id, UpdateVM updateVM);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<TrainerSelectVM>> GetTrainersForDropdown();
    Task<IEnumerable<CategorySelectVM>> GetCategoriesForDropdown();

}
