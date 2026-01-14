using GymManagementDataAccessLayer.Entities; 
using GymManagementDataAccessLayer.Repositories.Interfaces; 
using MapsterMapper;
using GymManagementBusinessLayer.Services.Interfaces;
using GymManagementBusinessLayer.ViewModels.TrainerVM;

namespace GymManagementBusinessLayer.Services.Classes;

public class TrainerService(IUnitOfWork unitOfWork, IMapper mapper) : ITrainerService
{
    public async Task<IEnumerable<TrainerVM>> GetAllAsync()
    {
        var trainers = await unitOfWork.Trainers.GetAllAsync(null, t => t.Address);

        return mapper.Map<IEnumerable<TrainerVM>>(trainers);
    }
    public async Task<bool> CreateAsync(CreateVM createVM)
    {
        try
        {
            if (createVM == null)
            {
                return false;
            }
            var EmailOrPhoneExists = await unitOfWork.Trainers.GetAsync(t => t.Email == createVM.Email || t.PhoneNumber == createVM.PhoneNumber);
            if (EmailOrPhoneExists != null)
            {
                return false;
            }
            var trainer = mapper.Map<Trainer>(createVM);
            await unitOfWork.Trainers.AddAsync(trainer);
            await unitOfWork.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Failed To Create Trainer {ex.Message}");
            return false;
        }

        return true;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var trainer = await unitOfWork.Trainers.GetAsync(
            filter: t => t.Id == id, "Sessions");
        if (trainer == null) return false;
        var futureSessions = trainer.Sessions.Any(s=>s.CreatedAt > DateTime.Now);
        if (futureSessions)
        {
            return false;
        }
        unitOfWork.Trainers.Delete(trainer);
        await unitOfWork.SaveChangesAsync();
        return true;
    }
    public async Task<DetailsVM?> GetDetailsAsync(int id)
    {
        var trainer = await unitOfWork.Trainers.GetAsync(
            filter: t => t.Id == id,
            includeProperties: "Address");
        if (trainer == null) return null;
        return mapper.Map<DetailsVM>(trainer);
    }
    public async Task<UpdateVM?> GetUpdateAsync(int id)
    {
        var trainer = await unitOfWork.Trainers.GetAsync(
            filter: t => t.Id == id,
            includeProperties: "Address");
        if (trainer == null) return null;
        return mapper.Map<UpdateVM>(trainer);
    }
    public async Task<bool> UpdateAsync(int id, UpdateVM updateVM)
    {
        var EmailOrPhoneExists = await unitOfWork.Trainers.GetAsync(t => (t.Id != id && (t.Email == updateVM.Email || t.PhoneNumber == updateVM.PhoneNumber) ));
        if (EmailOrPhoneExists != null)
        {
            return false;
        }
        var trainer = await unitOfWork.Trainers.GetAsync(
           filter: t => t.Id == id,
           includeProperties: "Address");
        if (trainer == null) return false;
        trainer.Email = updateVM.Email;
        trainer.PhoneNumber = updateVM.PhoneNumber;
        trainer.Address.BuildingNumber = updateVM.BuildingNumber;
        trainer.Address.Street = updateVM.Street;
        trainer.Address.City = updateVM.City;
        trainer.Specialty = updateVM.Specialization;

        unitOfWork.Trainers.Update(trainer);
        await unitOfWork.SaveChangesAsync();
        return true;

    }
}