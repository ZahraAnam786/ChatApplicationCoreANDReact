using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Domain.UnitOfWork
{
    public interface IUnitOfWork
    {
        int SaveChanges();
        int ExecuteSqlCommand(string sql, params object[] parameters);
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;
        int? CommandTimeout { get; set; }
        Task BeginTransactionAsync();
      //  void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        bool Commit();
        void Rollback();
    }
}
