using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementDAL.Data.Configurations
{
	internal class SessionConfigurations : IEntityTypeConfiguration<SessionEntity>
	{
		public void Configure(EntityTypeBuilder<SessionEntity> builder)
		{
			builder.ToTable(T =>
			{
				T.HasCheckConstraint("SessionCapacityConstraint", "Capacity between 1 and 25");
				T.HasCheckConstraint("SessionEndDateAfterStartDate", "EndDate > StartDate");

			});

			builder.HasOne(X => X.Trainer)
				.WithMany(X => X.Sessions)
				.HasForeignKey(X => X.TrainerId);

			builder.HasOne(X => X.Category)
				.WithMany(X => X.Sessions)
				.HasForeignKey(X => X.CategoryId);


		}
	}
}
