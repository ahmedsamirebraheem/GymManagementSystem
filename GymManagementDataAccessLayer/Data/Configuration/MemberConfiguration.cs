using GymManagementDataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementDataAccessLayer.Data.Configuration;

public class MemberConfiguration : GymUserConfiguration<Member> , IEntityTypeConfiguration<Member>
{
    public new void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.Property(m => m.CreatedAt)
               .HasColumnName("JoinDate")
               .HasDefaultValueSql("GETDATE()");

        base.Configure(builder);
    }

}
