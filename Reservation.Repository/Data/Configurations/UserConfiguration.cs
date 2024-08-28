using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reservation.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Repository.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasMany(U => U.ParentsSeats)
                .WithOne(S => S.User)
                .HasForeignKey(S => S.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(U => U.KidsSeat)
                .WithOne(S => S.User)
                .HasForeignKey(S => S.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
