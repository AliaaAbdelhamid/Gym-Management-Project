using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
	internal interface IGenericRepository<TEntity> where TEntity  : BaseEntity
	{
		TEntity? GetById(int id);
		IEnumerable<TEntity> GetAll();
		int Add(TEntity entity);
		int Update(TEntity entity);
		int Delete(TEntity entity);

	}
}
