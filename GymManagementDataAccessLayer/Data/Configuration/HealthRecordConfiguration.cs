using GymManagementDataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace GymManagementDataAccessLayer.Data.Configuration;

public class HealthRecordConfiguration : IEntityTypeConfiguration<HealthRecord>
{
    public void Configure(EntityTypeBuilder<HealthRecord> builder)
    {
        builder.ToTable("Members")
            .HasKey(x => x.Id);

        builder.HasOne<Member>()
            .WithOne(x=>x.HealthRecord)
            .HasForeignKey<HealthRecord>(x => x.Id);

        builder.Property(h => h.Height).HasPrecision(5, 2);
        builder.Property(h => h.Weight).HasPrecision(5, 2);

        builder.Ignore(x => x.CreatedAt);
    }
}
