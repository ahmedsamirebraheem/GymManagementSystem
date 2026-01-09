using GymManagementDataAccessLayer.Data.Context;
using GymManagementDataAccessLayer.Entities;
using GymManagementDataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementDataAccessLayer.Repositories.Classes;

public class MemberSessionRepository : GenericRepository<MemberSession>, IMemberSessionRepository
{
    private readonly GymDBContext _dbContext;
    public MemberSessionRepository(GymDBContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
