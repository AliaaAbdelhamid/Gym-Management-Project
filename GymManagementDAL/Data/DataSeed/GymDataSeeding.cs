using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GymManagementDAL.Data.DataSeed
{
	public static class GymDataSeeding
	{
		public static bool SeedData(GymDbContext dbContext)
		{
			try
			{
				bool HasCategories = dbContext.Categories.Any();
				bool HasPlans = dbContext.Plans.Any();
				if (HasCategories && HasPlans) return false;

				if (!HasCategories)
				{
					var Categories = LoadDataFromJsonFile<CategoryEntity>("categories.json");
					dbContext.Categories.AddRange(Categories);
				}

				if (!HasPlans)
				{
					var Plans = LoadDataFromJsonFile<PlanEntity>("plans.json");
					dbContext.Plans.AddRange(Plans);
				}

				int RowsAffected = dbContext.SaveChanges();
				return RowsAffected > 0;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Seeding Failed : {ex}");
				return false;
			}
		}

		private static List<T> LoadDataFromJsonFile<T>(string fileName)
		{
			var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", fileName);

			if (!File.Exists(filePath)) throw new FileNotFoundException();

			string Data = File.ReadAllText(filePath);
			var Options = new JsonSerializerOptions()
			{
				PropertyNameCaseInsensitive = true
			};

			Options.Converters.Add(new JsonStringEnumConverter());
			return JsonSerializer.Deserialize<List<T>>(Data, Options) ?? new List<T>();

		}
	}
}
