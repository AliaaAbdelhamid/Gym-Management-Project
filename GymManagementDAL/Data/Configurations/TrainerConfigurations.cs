using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Configurations
{
    internal class TrainerConfigurations : GymUserConfigurations<TrainerEntity>, IEntityTypeConfiguration<TrainerEntity>
    {
        public new void Configure(EntityTypeBuilder<TrainerEntity> builder)
        {
            builder.Property(X => X.CreatedAt)
                   .HasColumnName("HireDate")
                   .HasDefaultValueSql("GETDATE()");

            base.Configure(builder);
        }
    }
}
