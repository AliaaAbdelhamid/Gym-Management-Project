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
	internal class GymUserConfigurations<T> : IEntityTypeConfiguration<T> where T : GymUser
	{
		public void Configure(EntityTypeBuilder<T> builder)
		{
			builder.Property(X => X.CreatedAt)
				   .HasColumnName("JoinDate")
				   .HasDefaultValueSql("GETDATE()");

			builder.Property(X => X.Name)
					.HasColumnType("varchar(50)");

			builder.OwnsOne(x => x.Address, address =>
			{
				address.Property(a => a.Street)
					   .HasColumnType("varchar(30)")
					   .HasColumnName("Street");

				address.Property(a => a.City)
					   .HasColumnType("varchar(30)")
					   .HasColumnName("City");

				address.Property(a => a.BuildingNumber)
					   .HasColumnName("BuildingNumber");
			});

			builder.Property(X => X.Email)
				   .HasColumnType("varchar(100)");

			builder.Property(X => X.Phone)
				   .HasColumnType("varchar(11)");
		}
	}
}
