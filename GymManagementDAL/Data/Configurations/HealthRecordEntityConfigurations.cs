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
    internal class HealthRecordEntityConfigurations : IEntityTypeConfiguration<HealthRecordEntity>
    {
        public void Configure(EntityTypeBuilder<HealthRecordEntity> builder)
        {
            builder.Ignore(X => X.CreatedAt);
            builder.Ignore(X => X.UpdatedAt);
        }
    }
}
