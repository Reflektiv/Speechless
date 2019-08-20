using System.Transactions;

namespace Speechless.Infrastructure.Repositories.LiteDB.Extensions
{
    public static class TransactionExtensions
    {
        public static TransactionScope CreateTransactionScope(this TransactionScopeOption scopeOption, IsolationLevel level = IsolationLevel.ReadCommitted)
        {
            var options = new TransactionOptions
            {
                IsolationLevel = level,
                Timeout = TransactionManager.DefaultTimeout
            };
            return new TransactionScope(scopeOption, options);
        }

        public static TransactionScope CreateTransactionScopeFlow(this TransactionScopeOption scopeOption, IsolationLevel level = IsolationLevel.ReadCommitted)
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