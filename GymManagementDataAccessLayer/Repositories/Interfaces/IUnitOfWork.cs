using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementDataAccessLayer.Repositories.Interfaces;

public interface IUnitOfWork 
{
    IMemberRepository Members { get; }
    ICategoryRepository Categories { get; }
    ISessionRepository Sessions { get; }
    IHealthRecordRepository HealthRecords { get; }
    IPlanRepository Plans { get; }
    ITrainerRepository Trainers { get; }
    IMembershipRepository Memberships { get; }
    IMemberSessionRepository MemberSessions { get; }


    public Task<int> SaveChangesAsync();
}
