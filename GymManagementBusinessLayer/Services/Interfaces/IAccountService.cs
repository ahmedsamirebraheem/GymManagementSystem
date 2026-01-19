using GymManagementBusinessLayer.ViewModels.AccountVM;
using GymManagementDataAccessLayer.Entities;

namespace GymManagementBusinessLayer.Services.Interfaces;

public interface IAccountService
{
    Task<ApplicationUser?> ValidateUserAsync(LoginVM loginVM);
}
