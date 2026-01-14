using GymManagementDataAccessLayer.Data.Context;
using GymManagementDataAccessLayer.Entities;
using GymManagementDataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementDataAccessLayer.Repositories.Classes;

public class CategoryRepository(GymDBContext dbContext) : GenericRepository<Category>(dbContext), ICategoryRepository
{
    private readonly GymDBContext _dbContext = dbContext;
}
