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
    public static class EfCoreExtensions
    {
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