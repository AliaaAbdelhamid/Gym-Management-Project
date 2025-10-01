using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementDAL.Data.Configurations
{
	internal class MemberConfigurations : GymUserConfigurations<MemberEntity>, IEntityTypeConfiguration<MemberEntity>
	{
		public new void Configure(EntityTypeBuilder<MemberEntity> builder)
		{
			builder.Property(X => X.CreatedAt)
				   .HasColumnName("JoinDate")
				   .HasDefaultValueSql("GETDATE()");
			builder.HasOne(M => M.HealthRecord)
				   .WithOne()
				   .HasForeignKey<HealthRecordEntity>(M => M.Id);

			base.Configure(builder);

		}
	}
}
