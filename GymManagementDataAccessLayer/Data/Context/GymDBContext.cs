using GymManagementDataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GymManagementDataAccessLayer.Data.Context;

public class GymDBContext : DbContext
{
    public GymDBContext(DbContextOptions<GymDBContext> options) : base(options)
    {
        
    }
    protected override  void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
       

        

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                var method = typeof(GymDBContext)
                    .GetMethod(nameof(ApplySoftDeleteFilter), BindingFlags.NonPublic | BindingFlags.Static)
                    ?.MakeGenericMethod(entityType.ClrType);

                method?.Invoke(null, new object[] { modelBuilder });
            }
        }
    }

    public DbSet<Member> Members { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<MemberSession> MemberSessions { get; set; }
    public DbSet<Trainer> Trainers { get; set; }
    public DbSet<HealthRecord> HealthRecords { get; set; }
    public DbSet<Plan> Plans { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Membership> Memberships { get; set; }

    private static void ApplySoftDeleteFilter<T>(ModelBuilder modelBuilder) where T : BaseEntity
    {
        modelBuilder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
    }

}

