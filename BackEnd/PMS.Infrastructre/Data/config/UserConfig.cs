using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Infrastructre.Data.config
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            //builder.Property(x => x.Email)
            //    .IsRequired()
            //    .HasMaxLength(150);

            //builder.HasIndex(x => x.Email)
            //    .IsUnique();

            //builder.Property(x => x.PasswordHash)
            //    .IsRequired();

            builder.Property(x => x.Name)
                .HasMaxLength(100);
        }
    }
}
