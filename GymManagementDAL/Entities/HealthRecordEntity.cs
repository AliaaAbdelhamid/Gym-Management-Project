using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
	[Table("Members")]
	internal class HealthRecordEntity
	{
		[Key]
		public int Id { get; set; } // Shared PK - FK 
		public decimal Height  { get; set; }
		public decimal Weight  { get; set; }
		public string BloodType { get; set; } = null!;
		public string Note { get; set; } = null!;
	}
}
