using Couchbase.Lite;
using Couchbase.Lite.Query;
using reexmonkey.xmisc.backbone.identifiers.contracts.models;
using Reflektiv.Speechless.Core.Domain.Concretes.Models;
using Speechless.Core.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using cbExpression = Couchbase.Lite.Query.Expression;

namespace Speechless.Infrastructure.Repositories.CouchbaseLite
{
    public class BusinessCardRepository : IBusinessCardRepository
    {
        private readonly Database db;

        public BusinessCardRepository(IKeyGenerator<SequentialGuid> generator, Database db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public bool ContainsKey(Guid key)
        {
            var doc = db.GetDocument(key.ToString());
            return doc != null;
        }

        public Task<bool> ContainsKeyAsync(Guid key, CancellationToken token = default)
        {
            var tcs = new TaskCompletionSource<bool>();
            try
            {
                token.ThrowIfCancellationRequested();
                tcs.SetResult(ContainsKey(key));
            }
            catch (OperationCanceledException ex)
            {
                tcs.TrySetCanceled(ex.CancellationToken);
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }
            return tcs.Task;
        }

        public bool ContainsKeys(IEnumerable<Guid> keys, bool strict = true)
        {
            using (var query = QueryBuilder
                .SelectDistinct(SelectResult.Property("Id"))
                .From(DataSource.Database(db))
                .Where(cbExpression.Property("Id")
                .In(keys.Select(key => cbExpression.String(key.ToString())).ToArray())))
            {
                var results = query.Execute();
                return strict ? keys.Count() == results.Count() : results.Any();
            }
        }

        public Task<bool> ContainsKeysAsync(IEnumerable<Guid> keys, bool strict = true, CancellationToken token = default)
        {
            var tcs = new TaskCompletionSource<bool>();
            try
            {
                token.ThrowIfCancellationRequested();
                tcs.SetResult(ContainsKeys(keys));
            }
            catch (OperationCanceledException ex)
            {
                tcs.TrySetCanceled(ex.CancellationToken);
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }
            return tcs.Task;
        }

        public bool Erase(BusinessCard model) => EraseByKey(model.Id);

        public int EraseAll(IEnumerable<BusinessCard> models) => EraseAllByKeys(models.Select(x => x.Id));

        public Task<int> EraseAllAsync(IEnumerable<BusinessCard> models, CancellationToken token = default)
            => EraseAllByKeysAsync(models.Select(x => x.Id), token);

        public int EraseAllByKeys(IEnumerable<Guid> keys)
        {
            var deletes = 0;
            foreach (var key in keys)
            {
                if (EraseByKey(key)) ++deletes;
            }
            return deletes;
        }

        public Task<int> EraseAllByKeysAsync(IEnumerable<Guid> keys, CancellationToken token = default)
        {
            var tcs = new TaskCompletionSource<int>();
            try
            {
                token.ThrowIfCancellationRequested();
                tcs.SetResult(EraseAllByKeys(keys));
            }
            catch (OperationCanceledException ex)
            {
                tcs.TrySetCanceled(ex.CancellationToken);
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }
            return tcs.Task;
        }

        public Task<bool> EraseAsync(BusinessCard model, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public bool EraseByKey(Guid key)
        {
            var id = key.ToString();
            var doc = db.GetDocument(id);
            if (doc != null)
            {
                db.Purge(doc);
                return true;
            }
            return false;
        }

        public Task<bool> EraseByKeyAsync(Guid key, CancellationToken token = default)
        {
            var tcs = new TaskCompletionSource<bool>();
            try
            {
                token.ThrowIfCancellationRequested();
                tcs.SetResult(EraseByKey(key));
            }
            catch (OperationCanceledException ex)
            {
                tcs.TrySetCanceled(ex.CancellationToken);
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }
            return tcs.Task;
        }

        public IEnumerable<BusinessCard> FindAll(Expression<Func<BusinessCard, bool>> predicate, bool? references = null, int? offset = null, int? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BusinessCard>> FindAllAsync(Expression<Func<BusinessCard, bool>> predicate, bool? references = null, int? offset = null, int? count = null, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BusinessCard> FindAllByKeys(IEnumerable<Guid> keys, bool? references = null, int? offset = null, int? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BusinessCard>> FindAllByKeysAsync(IEnumerable<Guid> keys, bool? references = null, int? offset = null, int? count = null, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public BusinessCard FindByKey(Guid key, bool? references = null)
        {
            throw new NotImplementedException();
        }

        public Task<BusinessCard> FindByKeyAsync(Guid key, bool? references = null, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BusinessCard> Get(bool? references = null, int? offset = null, int? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BusinessCard>> GetAsync(bool? references = null, int? offset = null, int? count = null, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Guid> GetKeys(int? offset = null, int? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Guid>> GetKeysAsync(int? offset = null, int? count = null, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public void Register(BusinessCard model, bool? references = null)
        {
            throw new NotImplementedException();
        }

        public void RegisterAll(IEnumerable<BusinessCard> models, bool? references = null, int? offset = null, int? count = null)
        {
            throw new NotImplementedException();
        }

        public Task RegisterAllAsync(IEnumerable<BusinessCard> models, bool? references = null, int? offset = null, int? count = null, CancellationToken cancellation = default)
        {
            throw new NotImplementedException();
        }

        public Task RegisterAsync(BusinessCard model, bool? references = null, CancellationToken cancellation = default)
        {
            throw new NotImplementedException();
        }

        public void Restore(BusinessCard model, bool? references = null)
        {
            throw new NotImplementedException();
        }

        public void RestoreAll(IEnumerable<BusinessCard> models, bool? references = null)
        {
            throw new NotImplementedException();
        }

        public Task RestoreAllAsync(IEnumerable<BusinessCard> models, bool? references = null, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BusinessCard> RestoreAllByKeys(IEnumerable<Guid> keys, bool? references = null, int? offset = null, int? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BusinessCard>> RestoreAllByKeysAsync(IEnumerable<Guid> keys, bool? references = null, int? offset = null, int? count = null, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task RestoreAsync(BusinessCard model, bool? references = null)
        {
            throw new NotImplementedException();
        }

        public BusinessCard RestoreByKey(Guid key, bool? references = null)
        {
            throw new NotImplementedException();
        }

        public Task<BusinessCard> RestoreByKeyAsync(Guid key, bool? references = null)
        {
            throw new NotImplementedException();
        }

        public bool Save(BusinessCard model, bool references = true)
        {
            throw new NotImplementedException();
        }

        public int SaveAll(IEnumerable<BusinessCard> models, bool references = true)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveAllAsync(IEnumerable<BusinessCard> models, bool references = true, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveAsync(BusinessCard model, bool references = true, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public void Trash(BusinessCard model, bool? references = null)
        {
            throw new NotImplementedException();
        }

        public void TrashAll(IEnumerable<BusinessCard> models, bool? references = null)
        {
            throw new NotImplementedException();
        }

        public Task TrashAllAsync(IEnumerable<BusinessCard> models, bool? references = null, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BusinessCard> TrashAllByKeys(IEnumerable<Guid> keys, bool? references = null, int? offset = null, int? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BusinessCard>> TrashAllByKeysAsync(IEnumerable<Guid> keys, bool? references = null, int? offset = null, int? count = null, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task TrashAsync(BusinessCard model, bool? references = null)
        {
            throw new NotImplementedException();
        }

        public BusinessCard TrashByKey(Guid key, bool? references = null)
        {
            throw new NotImplementedException();
        }

        public Task<BusinessCard> TrashByKeyAsync(Guid key, bool? references = null, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public void Unregister(BusinessCard model, bool? references = null)
        {
            throw new NotImplementedException();
        }

        public void UnregisterAll(IEnumerable<BusinessCard> models, bool? references = null, int? offset = null, int? count = null)
        {
            throw new NotImplementedException();
        }

        public Task UnregisterAllAsync(IEnumerable<BusinessCard> models, bool? references = null, int? offset = null, int? count = null, CancellationToken cancellation = default)
        {
            throw new NotImplementedException();
        }

        public Task UnregisterAsync(BusinessCard model, bool? references = null, CancellationToken cancellation = default)
        {
            throw new NotImplementedException();
        }
    }
}
