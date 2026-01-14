using GymManagementDataAccessLayer.Data.Context;
using GymManagementDataAccessLayer.Entities;
using GymManagementDataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementDataAccessLayer.Repositories.Classes;

public class MemberSessionRepository(GymDBContext dbContext): GenericRepository<MemberSession>(dbContext), IMemberSessionRepository
{

    private readonly GymDBContext _dbContext = dbContext;

}
