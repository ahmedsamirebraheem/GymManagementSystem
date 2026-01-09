using GymManagementDataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GymManagementDataAccessLayer.Data.Configuration;

public class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(x => x.Name)
            .HasColumnType("varchar")
            .HasMaxLength(50);

        builder.Property(x => x.Email)
            .HasColumnType("varchar")
            .HasMaxLength(100);

        builder.Property(x => x.PhoneNumber)
            .HasColumnType("varchar")
            .HasMaxLength(11);

        builder.ToTable(tb =>
        {
            tb.HasCheckConstraint("GymUserValidEmailCheck", "Email LIKE '%_@_%._%' AND Email NOT LIKE '% %'");
            tb.HasCheckConstraint("GymUserValidPhoneCheck","PhoneNumber LIKE '01%' AND PhoneNumber NOT LIKE '%[^0-9]%' AND LEN(PhoneNumber) = 11"
);

        });

        builder.HasIndex(x => x.Email).IsUnique();
        builder.HasIndex(x => x.PhoneNumber).IsUnique();

        builder.OwnsOne(x => x.Address, addressBuilder =>
        {
            addressBuilder.Property(a => a.Street)
                .HasColumnType("varchar")
                .HasMaxLength(30)
                .HasColumnName("Street");

            addressBuilder.Property(a => a.City)
                .HasColumnType("varchar")
                .HasMaxLength(30)
                .HasColumnName("City");

            addressBuilder.Property(a => a.BuildingNumber)
                .HasColumnName("BuildingNumber");

        });


    }
}
