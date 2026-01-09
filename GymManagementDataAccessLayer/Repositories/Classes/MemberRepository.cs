using GymManagementDataAccessLayer.Data.Context;
using GymManagementDataAccessLayer.Entities;
using GymManagementDataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementDataAccessLayer.Repositories.Classes;

public class MemberRepository : GenericRepository<Member>, IMemberRepository
{
    private readonly GymDBContext _dbContext;
    public MemberRepository(GymDBContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
