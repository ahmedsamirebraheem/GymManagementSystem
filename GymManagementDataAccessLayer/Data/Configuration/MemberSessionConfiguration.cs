
using GymManagementDataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace  GymManagementDataAccessLayer.Data.Configuration;


public class MemberSessionConfiguration : IEntityTypeConfiguration<MemberSession>
{
    public void Configure(EntityTypeBuilder<MemberSession> builder)
    {
        builder.Property(x => x.CreatedAt)
            .HasColumnName("BookingDate")
            .HasDefaultValueSql("GETDATE()");

        

        builder.HasKey(x => x.Id);

        builder.HasIndex(m => new { m.MemberId, m.SessionId }).IsUnique();

    }
}
