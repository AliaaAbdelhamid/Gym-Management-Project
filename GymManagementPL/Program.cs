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

			builder.Services.AddDbContext<GymDbContext>();
			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
			builder.Services.AddScoped<IMemberService, MemberService>();
			builder.Services.AddScoped<IPlanService, PlanService>();
			builder.Services.AddAutoMapper(M=>M.AddProfile(new MappingProfile()));
			var app = builder.Build();

			#region Migrate Database -  Data Seeding
			using var Scoope = app.Services.CreateScope();
			var DbContextObj = Scoope.ServiceProvider.GetRequiredService<GymDbContext>();
			var PendingMigrations = DbContextObj.Database.GetPendingMigrations();
			if (PendingMigrations.Any())
				DbContextObj.Database.Migrate();
			GymDataSeeding.SeedData(DbContextObj);
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
