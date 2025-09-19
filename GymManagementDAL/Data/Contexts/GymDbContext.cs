using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Contexts
{
	internal class GymDbContext:DbContext
	{

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Server=.;Database=GymManagement;Trusted_Connection=true;TrustServerCertificate=true");
		}


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		}

		#region DbSets
		public DbSet<TrainerEntity> Trainers { get; set; }
		public DbSet<BookingEntity> Bookings { get; set; }
		public DbSet<CategoryEntity> Categories { get; set; }
		public DbSet<HealthRecordEntity> HealthRecords { get; set; }
		public DbSet<MemberEntity> Members { get; set; }
		public DbSet<MembershipEntity> Memberships { get; set; }
		public DbSet<PlanEntity> Plans { get; set; }
		public DbSet<SessionEntity> Sessions { get; set; }
		#endregion



	}
}
