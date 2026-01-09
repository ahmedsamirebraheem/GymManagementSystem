using GymManagementDataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementDataAccessLayer.Data.Configuration;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.ToTable(tb =>
        {
            tb.HasCheckConstraint("SessionCapacity_Check", "Capacity Between 1 and 25");
            tb.HasCheckConstraint("SessionEndDate", "EndDate > StartDate");
        });

        builder.HasOne(s => s.Category)
               .WithMany(c => c.Sessions)
               .HasForeignKey(s => s.CategoryId);
        
        builder.HasOne(s => s.Trainer)
                .WithMany(t => t.Sessions)
                .HasForeignKey(s => s.TrainerId);


    }


}
