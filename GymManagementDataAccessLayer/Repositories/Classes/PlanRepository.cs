using GymManagementDataAccessLayer.Data.Context;
using GymManagementDataAccessLayer.Entities;
using GymManagementDataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementDataAccessLayer.Repositories.Classes;

public class PlanRepository : GenericRepository<Plan>, IPlanRepository
{
    private readonly GymDBContext _dbContext;
    public PlanRepository(GymDBContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
