using GymManagementDataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace GymManagementDataAccessLayer.Data.Configuration;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(x => x.Name)
                   .HasColumnType("varchar")
                   .HasMaxLength(20);

        builder.HasIndex(c => c.Name)
            .IsUnique();
    }
}
