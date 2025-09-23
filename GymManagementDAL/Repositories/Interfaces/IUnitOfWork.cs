using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
	internal interface IUnitOfWork : IDisposable
	{
		IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity ; 
		int SaveChanges();

	}
}
