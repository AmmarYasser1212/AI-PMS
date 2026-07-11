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
    public class ScheduleTaskConfig : IEntityTypeConfiguration<ScheduleTask>
    {
        public void Configure(EntityTypeBuilder<ScheduleTask> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Schedule)
                .WithMany(x => x.ScheduleTasks)
                .HasForeignKey(x => x.ScheduleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Task)
                .WithMany()
                .HasForeignKey(x => x.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
