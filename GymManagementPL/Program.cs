using GymManagementBLL;
using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Data.DataSeed;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementPL
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();

			builder.Services.AddDbContext<GymDbContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
			});
			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
			builder.Services.AddScoped<IMembershipRepository, MembershipRepository>();
			builder.Services.AddScoped<ISessionRepository, SessionRepository>();
			builder.Services.AddScoped<IMemberService, MemberService>();
			builder.Services.AddScoped<ITrainerService, TrainerService>();
			builder.Services.AddScoped<IPlanService, PlanService>();
			builder.Services.AddScoped<ISessionService, SessionService>();
			builder.Services.AddScoped<IMembershipService, MembershipService>();
			builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();



			builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfile()));
			var app = builder.Build();

			#region Migrate Database -  Data Seeding
			using var Scope = app.Services.CreateScope();
			var dbContextObj = Scope.ServiceProvider.GetRequiredService<GymDbContext>();
			var PendingMigrations = dbContextObj.Database.GetPendingMigrations();
			if (PendingMigrations?.Any() ?? false)
				dbContextObj.Database.Migrate();
			GymDataSeeding.SeedData(dbContextObj);
			#endregion

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseRouting();

			app.UseAuthorization();

			app.MapStaticAssets();
			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}")
				.WithStaticAssets();

			app.Run();
		}
	}
}
