using GymManagementDataAccessLayer.Data.Context;
using GymManagementDataAccessLayer.Entities;
using GymManagementDataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementDataAccessLayer.Repositories.Classes;

public class TrainerRepository : GenericRepository<Trainer>, ITrainerRepository
{
    private readonly GymDBContext _dbContext;
    public TrainerRepository(GymDBContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
