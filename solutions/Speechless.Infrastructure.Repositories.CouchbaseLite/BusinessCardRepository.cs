using Couchbase.Lite;
using Couchbase.Lite.Query;
using reexmonkey.xmisc.backbone.identifiers.contracts.models;
using reexmonkey.xmisc.backbone.io.jil.serializers;
using reexmonkey.xmisc.core.io.serializers;
using Reflektiv.Speechless.Core.Domain.Concretes.Models;
using Speechless.Core.Repositories.Contracts;
using Speechless.Infrastructure.Repositories.CouchbaseLite.Extensions;
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
        private readonly IKeyGenerator<SequentialGuid> generator;
        private readonly Database db;
        private readonly TextSerializerBase serializer;

        public BusinessCardRepository(IKeyGenerator<SequentialGuid> generator, Database db, TextSerializerBase serializer)
        {
            this.generator = generator ?? throw new ArgumentNullException(nameof(generator));
            this.db = db ?? throw new ArgumentNullException(nameof(db));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
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
            var ids = keys.Select(key => cbExpression.String(key.ToString())).ToArray();
            using (var query = QueryBuilder
                .SelectDistinct(SelectResult.Property("Id"))
                .From(DataSource.Database(db))
                .Where(cbExpression.Property("Id")
                .In(ids)))
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
            db.InBatch(() =>
            {
                foreach (var key in keys)
                {
                    var doc = db.GetDocument(key.ToString());
                    if (doc == null) continue;
                    db.Purge(doc);
                    ++deletes;
                }
            });
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
            var doc = db.GetDocument(key.ToString());
            if (doc != null)
            {
                db.Delete(doc, ConcurrencyControl.LastWriteWins);
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
            var filter = predicate.AsExpression();
            IResultSet results = null;
            if (offset != null && count != null)
            {
                using (var query = QueryBuilder
                    .Select(SelectResult.All())
                    .From(DataSource.Database(db))
                    .Where(filter)
                    .Limit(cbExpression.Int(count.Value), cbExpression.Int(offset.Value)))
                    results = query.Execute();
            }
            else
            {
                using (var query = QueryBuilder
                    .Select(SelectResult.All())
                    .From(DataSource.Database(db))
                    .Where(filter))
                    results = query.Execute();
            }
            return results != null
                ? results.Select(result => result.GetDictionary(db.Name).AsBusinessCard(serializer))
                : Enumerable.Empty<BusinessCard>();
        }

        public Task<IEnumerable<BusinessCard>> FindAllAsync(Expression<Func<BusinessCard, bool>> predicate, bool? references = null, int? offset = null, int? count = null, CancellationToken token = default)
        {
            var tcs = new TaskCompletionSource<IEnumerable<BusinessCard>>();
            try
            {
                token.ThrowIfCancellationRequested();
                tcs.SetResult(FindAll(predicate, references, offset, count));
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

        public IEnumerable<BusinessCard> FindAllByKeys(IEnumerable<Guid> keys, bool? references = null, int? offset = null, int? count = null)
            => FindAll(x => keys.Contains(x.Id), references, offset, count);

        public Task<IEnumerable<BusinessCard>> FindAllByKeysAsync(IEnumerable<Guid> keys, bool? references = null, int? offset = null, int? count = null, CancellationToken token = default)
        {
            var tcs = new TaskCompletionSource<IEnumerable<BusinessCard>>();
            try
            {
                token.ThrowIfCancellationRequested();
                tcs.SetResult(FindAllByKeys(keys, references, offset, count));
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

        public BusinessCard FindByKey(Guid key, bool? references = null)
        {
            var doc = db.GetDocument(key.ToString());
            return doc != null ? doc.AsBusinessCard(serializer) : default;
        }

        public Task<BusinessCard> FindByKeyAsync(Guid key, bool? references = null, CancellationToken token = default)
        {
            var tcs = new TaskCompletionSource<BusinessCard>();
            try
            {
                token.ThrowIfCancellationRequested();
                tcs.SetResult(FindByKey(key, references));
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

        public IEnumerable<BusinessCard> Get(bool? references = null, int? offset = null, int? count = null)
            => FindAll(x => true, references, offset, count);

        public Task<IEnumerable<BusinessCard>> GetAsync(bool? references = null, int? offset = null, int? count = null, CancellationToken token = default)
        {
            var tcs = new TaskCompletionSource<IEnumerable<BusinessCard>>();
            try
            {
                token.ThrowIfCancellationRequested();
                tcs.SetResult(Get(references, offset, count));
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

        public IEnumerable<Guid> GetKeys(int? offset = null, int? count = null)
        {
            IResultSet results = null;
            if (offset != null && count != null)
            {
                using (var query = QueryBuilder
                    .Select(SelectResult.Property("Id"))
                    .From(DataSource.Database(db))
                    .Limit(cbExpression.Int(count.Value), cbExpression.Int(offset.Value)))
                    results = query.Execute();
            }
            else
            {
                using (var query = QueryBuilder
                    .Select(SelectResult.Property("Id"))
                    .From(DataSource.Database(db)))
                    results = query.Execute();
            }
            return results != null
                ? results.Select(result => result.GetDictionary(db.Name).As(serializer))
                : Enumerable.Empty<BusinessCard>();
        }

        public Task<IEnumerable<Guid>> GetKeysAsync(int? offset = null, int? count = null, CancellationToken token = default)
        {
            var tcs = new TaskCompletionSource<IEnumerable<Guid>>();
            try
            {
                token.ThrowIfCancellationRequested();
                tcs.SetResult(GetKeys(offset, count));
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

        public void Register(BusinessCard model, bool? references = null) => model.Id = generator.GetNext();

        public void RegisterAll(IEnumerable<BusinessCard> models, bool? references = null, int? offset = null, int? count = null)
        {
            foreach (var model in models) Register(model, references);
        }

        public Task RegisterAllAsync(IEnumerable<BusinessCard> models, bool? references = null, int? offset = null, int? count = null, CancellationToken cancellation = default)
        {
            var exceptions = new List<Exception>();
            try
            {
                foreach (var model in models)
                {
                    cancellation.ThrowIfCancellationRequested();
                    Register(model, references);
                }
            }
            catch (OperationCanceledException ex)
            {
                exceptions.Add(ex);
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
            return exceptions.Any()
                ? Task.FromException<AggregateException>(new AggregateException(exceptions))
                : Task.CompletedTask;
        }

        public Task RegisterAsync(BusinessCard model, bool? references = null, CancellationToken cancellation = default)
        {
            var tcs = new TaskCompletionSource<bool>();
            try
            {
                cancellation.ThrowIfCancellationRequested();
                Register(model, references);
                tcs.TrySetResult(true);
            }
            catch (OperationCanceledException)
            {
                tcs.TrySetCanceled(cancellation);
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }
            return tcs.Task;
        }

        public void Restore(BusinessCard model, bool? references = null)
        {
            model.IsDeleted = false;
            Save(model, references ?? false);
        }

        public void RestoreAll(IEnumerable<BusinessCard> models, bool? references = null)
        {
            foreach (var model in models) model.IsDeleted = false;
            SaveAll(models, references ?? false);
        }

        public Task RestoreAllAsync(IEnumerable<BusinessCard> models, bool? references = null, CancellationToken token = default)
        {
            foreach (var model in models) model.IsDeleted = false;
            return SaveAllAsync(models, references ?? false, token);
        }

        public IEnumerable<BusinessCard> RestoreAllByKeys(IEnumerable<Guid> keys, bool? references = null, int? offset = null, int? count = null)
        {
            var matches = FindAllByKeys(keys, references, offset, count);
            if (matches.Any()) RestoreAll(matches, references);
            return matches;
        }

        public async Task<IEnumerable<BusinessCard>> RestoreAllByKeysAsync(IEnumerable<Guid> keys, bool? references = null, int? offset = null, int? count = null, CancellationToken token = default)
        {
            var matches = await FindAllByKeysAsync(keys, references, offset, count, token).ConfigureAwait(false);
            if (matches.Any()) await RestoreAllAsync(matches, references, token).ConfigureAwait(false);
            return matches;
        }

        public Task RestoreAsync(BusinessCard model, bool? references = null)
        {
            model.IsDeleted = false;
            return SaveAsync(model, references ?? false);
        }

        public BusinessCard RestoreByKey(Guid key, bool? references = null)
        {
            var match = FindByKey(key, references);
            if (match != null) Restore(match, references);
            return match;
        }

        public async Task<BusinessCard> RestoreByKeyAsync(Guid key, bool? references = null)
        {
            var match = await FindByKeyAsync(key, references).ConfigureAwait(false);
            if (match != null) await RestoreAsync(match, references).ConfigureAwait(false);
            return match;
        }

        public bool Save(BusinessCard model, bool references = true)
        {
            var serializer = new JilTextSerializer();
            var content = serializer.Serialize(model);
            using (var doc = new MutableDocument(model.Id.ToString()))
            {
                doc.SetString("content", content);
            }
        }

        public int SaveAll(IEnumerable<BusinessCard> models, bool references = true)
        {
            db.InBatch(() =>
            {
                foreach (var key in keys)
                {
                    var doc = db.GetDocument(key.ToString());
                    if (doc == null) continue;
                    db.Purge(doc);
                    ++deletes;
                }
            });
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
