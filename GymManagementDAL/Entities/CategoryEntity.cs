namespace GymManagementDAL.Entities
{
	public class CategoryEntity : BaseEntity
	{
		public string CategoryName { get; set; } = null!;

		public ICollection<SessionEntity> Sessions { get; set; } = null!;
	}
}
