using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
