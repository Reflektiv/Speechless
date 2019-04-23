using Microsoft.EntityFrameworkCore;
using Reflektiv.Speechless.Core.Domain.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace PwC.xEric.Infrastructure.Repositories.Ormlite.Extensions
{
    public static class OrmliteExtensions
    {
        //public static void DropTableIfExists<TModel>(this IDbConnection db)
        //{
        //    if (db.TableExists<TModel>()) db.DropTable<TModel>();
        //}

        //public static void DeleteOrCreateTable<TModel>(this IDbConnection db)
        //{
        //    if (db.TableExists<TModel>()) db.DeleteAll<TModel>();
        //    else db.CreateTable<TModel>();
        //}

        //public static Task DeleteOrCreateTableAsync<TModel>(this IDbConnection db, CancellationToken token = default)
        //{
        //    if (db.TableExists<TModel>()) db.DeleteAllAsync<TModel>(token);
        //    else db.CreateTable<TModel>();
        //    return Task.FromResult(true);
        //}


        public static TransactionScope CreateTransactionScope(TransactionScopeOption scopeOption, IsolationLevel level = IsolationLevel.ReadCommitted)
        {
            var options = new TransactionOptions
            {
                IsolationLevel = level,
                Timeout = TransactionManager.DefaultTimeout
            };
            return new TransactionScope(scopeOption, options);
        }

        public static TransactionScope CreateTransactionScopeFlow(TransactionScopeOption scopeOption, IsolationLevel level = IsolationLevel.ReadCommitted)
        {
            var options = new TransactionOptions
            {
                IsolationLevel = level,
                Timeout = TransactionManager.DefaultTimeout
            };
            return new TransactionScope(scopeOption, options, TransactionScopeAsyncFlowOption.Enabled);
        }
    }
}