using GymManagementBusinessLayer.ViewModels.TrainerVM;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementBusinessLayer.Services.Interfaces;

public interface ITrainerService
{
    Task<IEnumerable<TrainerVM>> GetAllAsync();
    Task<bool> CreateAsync(CreateVM createVM);
    Task<DetailsVM?> GetDetailsAsync(int id);
    Task<UpdateVM?> GetUpdateAsync(int id);
    Task<bool> UpdateAsync(int id, UpdateVM updateVM);
    Task<bool> DeleteAsync(int id);


}
