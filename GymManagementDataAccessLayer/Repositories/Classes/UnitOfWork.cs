using GymManagementDataAccessLayer.Data.Context;
using GymManagementDataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementDataAccessLayer.Repositories.Classes;

public class UnitOfWork(GymDBContext dbContext) : IUnitOfWork , IDisposable
{
    private IMemberRepository? _members;
    private ICategoryRepository? _categories;
    private ISessionRepository? _sessions;
    private IHealthRecordRepository? _healthRecords;
    private IPlanRepository? _plans;
    private ITrainerRepository? _trainers;
    private IMembershipRepository? _memberships;
    private IMemberSessionRepository? _memberSessions;

    public IMemberRepository Members => _members ??= new MemberRepository(_dbContext);
    public ICategoryRepository Categories => _categories ??= new CategoryRepository(_dbContext);
    public ISessionRepository Sessions => _sessions ??= new SessionRepository(_dbContext);
    public IHealthRecordRepository HealthRecords => _healthRecords ??= new HealthRecordRepository(_dbContext);
    public IPlanRepository Plans => _plans ??= new PlanRepository(_dbContext);
    public ITrainerRepository Trainers => _trainers ??= new TrainerRepository(_dbContext);
    public IMembershipRepository Memberships => _memberships ??= new MembershipRepository(_dbContext);
    public IMemberSessionRepository MemberSessions => _memberSessions ??= new MemberSessionRepository(_dbContext);

    private readonly GymDBContext _dbContext = dbContext;
    

    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}
