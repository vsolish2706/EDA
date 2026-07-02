using Hotel.Reservations.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hotel.Reservations.Infrastructure.Configuration;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("Reservations");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.CustomerId)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(r => r.HotelId)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(r => r.RoomType)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(r => r.Guests)
            .IsRequired();

        builder.Property(r => r.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(32);

        builder.Property(r => r.StatusReason)
            .HasMaxLength(512);

        builder.OwnsOne(r => r.StayPeriod, stay =>
        {
            stay.Property(p => p.CheckIn)
                .HasColumnName("CheckIn")
                .IsRequired();

            stay.Property(p => p.CheckOut)
                .HasColumnName("CheckOut")
                .IsRequired();

            stay.Ignore(p => p.Nights);
        });

        builder.Navigation(r => r.StayPeriod).IsRequired();

        builder.Property(r => r.CreatedAt)
            .IsRequired();

        builder.Property(r => r.UpdatedAt);

        builder.HasIndex(r => r.CustomerId)
            .HasDatabaseName("IX_Reservations_CustomerId");

        builder.HasIndex(r => new { r.HotelId, r.Status })
            .HasDatabaseName("IX_Reservations_HotelId_Status");
    }
}

