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
    public class TimeTrackingConfig : IEntityTypeConfiguration<TimeTracking>
    {
        public void Configure(EntityTypeBuilder<TimeTracking> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Task)
                .WithMany(x => x.TimeTrackings)
                .HasForeignKey(x => x.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
