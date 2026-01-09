using GymManagementDataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace GymManagementDataAccessLayer.Data.Configuration;

public class PlanConfiguration : IEntityTypeConfiguration<Plan>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Plan> builder)
    {
        builder.Property(x => x.Name)
            .HasColumnType("varchar")
            .HasMaxLength(50);

        builder.Property(x => x.Description)
            .HasColumnType("varchar")
            .HasMaxLength(100);

        builder.Property(x => x.Price)
            .HasPrecision(10, 2);


        builder.ToTable(tb =>
        {
            tb.HasCheckConstraint("Plan_DurationDays_Check", "DurationDays Between 1 and 365");
        });
    }
}
