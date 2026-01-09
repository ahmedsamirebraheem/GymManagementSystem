using GymManagementDataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementDataAccessLayer.Data.Configuration;

public class TrainerConfiguration : GymUserConfiguration<Trainer>, IEntityTypeConfiguration<Trainer>
{
    public new void Configure(EntityTypeBuilder<Trainer> builder)
    {
        builder.Property(m => m.CreatedAt)
               .HasColumnName("HireDate")
               .HasDefaultValueSql("GETDATE()");

        base.Configure(builder);
    }

}
