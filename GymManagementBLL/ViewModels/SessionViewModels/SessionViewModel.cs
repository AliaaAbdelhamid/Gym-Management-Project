namespace GymManagementBLL.ViewModels.SessionViewModels
{
	public class SessionViewModel
	{
		public int Id { get; set; }
		public string Description { get; set; } = null!;
		public int Capacity { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string TrainerName { get; set; } = null!;
		public string CategoryName { get; set; } = null!;
		public int AvailableSlots { get; set; }

		// Computed properties
		public string DateRangeDisplay => $"{StartDate:MMM dd} - {EndDate:MMM dd, yyyy}";
		public string TimeRangeDisplay => $"{StartDate:hh:mm tt} - {EndDate:hh:mm tt}";
		public TimeSpan Duration => EndDate - StartDate;
		public bool IsUpcoming => StartDate > DateTime.Now;
		public bool IsOngoing => StartDate <= DateTime.Now && EndDate >= DateTime.Now;
		public bool IsCompleted => EndDate < DateTime.Now;
		public string StatusBadge => IsOngoing ? "Ongoing" : IsUpcoming ? "Upcoming" : "Completed";
	}
}
