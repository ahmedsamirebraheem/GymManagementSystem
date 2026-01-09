using GymManagementBusinessLayer.Services.Interfaces;
using GymManagementBusinessLayer.ViewModels.SessionVM;
using GymManagementDataAccessLayer.Entities;
using GymManagementDataAccessLayer.Repositories.Interfaces;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementBusinessLayer.Services.Classes;

public class SessionService(IUnitOfWork unitOfWork, IMapper mapper) : ISessionService
{
    
    public async Task<IEnumerable<SessionVM>> GetAllAsync()
    {
        var sessions = await unitOfWork.Sessions.GetAllAsync(
        filter: null,
        s => s.Category,
        s => s.Trainer,
        s => s.SessionMembers
    );
        if (sessions is null || !sessions.Any())
        {
            return [];
        }
        return mapper.Map<IEnumerable<SessionVM>>(sessions);
    }

    public async Task<SessionVM?> GetAsync(int id)
    {
        var session = await unitOfWork.Sessions.GetAsync(s => s.Id == id,
            s=>s.Category,
            s=>s.Trainer,
            s=>s.SessionMembers);

        if (session is null) return null;
        return mapper.Map<SessionVM>(session);
    }

    public async Task<bool> CreateAsync(CreateVM createVM)
    {
        try
        {
            if (createVM.StartDate >= createVM.EndDate)
            {
                return false;
            }

            var isBusy = await unitOfWork.Sessions.GetAsync(s =>
                s.TrainerId == createVM.TrainerId &&
                createVM.StartDate < s.EndDate &&
                createVM.EndDate > s.StartDate);

            if (isBusy != null) return false;

            var session = mapper.Map<Session>(createVM);
            await unitOfWork.Sessions.AddAsync(session);
            await unitOfWork.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Create Session Failed {ex}!.");
            return false;
        }

    }

    public async Task<UpdateVM?> GetUpdateAsync(int id)
    {
        var session = await unitOfWork.Sessions.GetAsync(s => s.Id == id, s => s.SessionMembers);
        if (session is null) return null;

        if (session.EndDate <= DateTime.Now) return null;

        return mapper.Map<UpdateVM>(session);
    }

    public async Task<bool> UpdateAsync(int id, UpdateVM updateVM)
    {
        try
        {
            var session = await unitOfWork.Sessions.GetAsync(s => s.Id == id, s => s.SessionMembers);
            if (session is null) return false;

            int currentBookedMembers = session.SessionMembers?.Count ?? 0;

            if (updateVM.Capacity < currentBookedMembers)
            {
                return false;
            }

            var trainerBusy = await unitOfWork.Sessions.GetAsync(s =>
                s.TrainerId == updateVM.TrainerId &&
                s.Id != id &&
                updateVM.StartDate < s.EndDate &&
                updateVM.EndDate > s.StartDate);

            if (trainerBusy != null) return false;

            mapper.Map(updateVM, session);
            session.UpdatedAt = DateTime.Now;

            unitOfWork.Sessions.Update(session);
            await unitOfWork.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Update Session Failed {ex}!.");
            return false;
        }
    }
    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var session = await unitOfWork.Sessions.GetAsync(s => s.Id == id, s => s.SessionMembers);

            if (session is null) return false;

            if (DateTime.Now >= session.StartDate)
            {
                return false;
            }

            if (session.SessionMembers != null && session.SessionMembers.Count > 0)
            {
                return false;
            }

            unitOfWork.Sessions.Delete(session);
            await unitOfWork.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Delete Session Failed: {ex.Message}");
            return false;
        }
    }

    public async Task<IEnumerable<TrainerSelectVM>> GetTrainersForDropdown()
    {
        var trainers = await unitOfWork.Trainers.GetAllAsync();

        return mapper.Map<IEnumerable<TrainerSelectVM>>(trainers);
    }

    public async Task<IEnumerable<CategorySelectVM>> GetCategoriesForDropdown()
    {
        var categories = await unitOfWork.Categories.GetAllAsync();
        return mapper.Map<IEnumerable<CategorySelectVM>>(categories);
    }
}
