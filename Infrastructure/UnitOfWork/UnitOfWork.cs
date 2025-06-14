using Domain.Interfaces;
using Domain.UnitOfWork;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWorkAsync
    {
        private readonly ApplicationDBContext _context;
        private IDbContextTransaction _Transaction;
        protected Dictionary<string, dynamic> Repositories;

        public UnitOfWork(ApplicationDBContext context)
        {
            _context = context;
            //_Transaction = Transaction;
            Repositories = new Dictionary<string, dynamic>();
        }

        public virtual IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            return RepositoryAsync<TEntity>();
        }

        public int? CommandTimeout
        {
            get => _context.Database.GetCommandTimeout();
            set => _context.Database.SetCommandTimeout(value);
        }

        public virtual int SaveChanges() => _context.SaveChanges();

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        public virtual IRepositoryAsync<TEntity> RepositoryAsync<TEntity>() where TEntity : class
        {

            if (Repositories == null)
            {
                Repositories = new Dictionary<string, dynamic>();
            }

            var type = typeof(TEntity).Name;

            if (Repositories.ContainsKey(type))
            {
                return (IRepositoryAsync<TEntity>)Repositories[type];
            }

            var repositoryType = typeof(Repository<>);

            Repositories.Add(type, Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context, this));

            return Repositories[type];
        }

        public virtual int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return _context.Database.ExecuteSqlRaw(sql, parameters);
        }

        public virtual async Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters)
        {
            return await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        public virtual async Task<int> ExecuteSqlCommandAsync(string sql, CancellationToken cancellationToken, params object[] parameters)
        {
            return await _context.Database.ExecuteSqlRawAsync(sql, cancellationToken, parameters);
        }

        //public async Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        //{
        //    var connection = _context.Database.GetDbConnection();

        //    if (connection.State != ConnectionState.Open)
        //        await connection.OpenAsync();

        //    var transaction = await connection.BeginTransactionAsync(isolationLevel);

        //    _context.Database.UseTransaction(transaction);
        //}

        //public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        //{
        //    var connection = _context.Database.GetDbConnection();

        //    if (connection.State != ConnectionState.Open)
        //        connection.Open();

        //    var transaction = connection.BeginTransaction(isolationLevel);

        //    _context.Database.UseTransaction(transaction);
        //}


        public async Task BeginTransactionAsync()
        {
            if (_context.Database.CurrentTransaction == null)
            {
                _Transaction = await _context.Database.BeginTransactionAsync();
            }
        }
        public virtual bool Commit()
        {
            _Transaction?.Commit();
            return true;
        }

        public virtual void Rollback()
        {
            _Transaction?.Rollback();
        }


    }
}
