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
	internal class SessionConfigurations : IEntityTypeConfiguration<SessionEntity>
	{
		public void Configure(EntityTypeBuilder<SessionEntity> builder)
		{
			builder.ToTable(T => T.HasCheckConstraint("SessionCapacityConstraint", "[Capacity] between 0 and 25"));

			builder.HasOne(X => X.Trainer)
				.WithMany(X => X.Sessions)
				.HasForeignKey(X => X.TrainerId);

			builder.HasOne(X => X.Category)
				.WithMany(X => X.Sessions)
				.HasForeignKey(X => X.CategoryId);


		}
	}
}
