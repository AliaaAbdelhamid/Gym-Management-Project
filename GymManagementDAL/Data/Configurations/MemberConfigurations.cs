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
	internal class MemberConfigurations : GymUserConfigurations<MemberEntity>, IEntityTypeConfiguration<MemberEntity>
	{
		public new void Configure(EntityTypeBuilder<MemberEntity> builder)
		{
			builder.HasOne(M => M.HealthRecord)
				   .WithOne()
				   .HasForeignKey<HealthRecordEntity>(M => M.Id);

			base.Configure(builder);
		}
	}
}
