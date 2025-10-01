using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


namespace GymManagementDAL.Data.Contexts
{
	public class GymDbContext : DbContext
	{

		public GymDbContext(DbContextOptions<GymDbContext> dbContextOptions) : base(dbContextOptions)
		{

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
