using GymManagementDataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace GymManagementDataAccessLayer.Data.Configuration;

public class MembershipConfiguration : IEntityTypeConfiguration<Membership>
{
    public void Configure(EntityTypeBuilder<Membership> builder)
    {
        builder.Property(x => x.CreatedAt)
            .HasColumnName("StartDate")
            .HasDefaultValueSql("GETDATE()");

        builder.HasIndex(m => new { m.MemberId, m.PlanId }).IsUnique();

        builder.HasKey(m => m.Id);
        builder.HasQueryFilter(m => !m.IsDeleted);
    }
}
