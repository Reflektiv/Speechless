using LiteDB;
using reexmonkey.xmisc.backbone.identifiers.contracts.models;
using Reflektiv.Speechless.Core.Domain.Concretes.Models;
using Speechless.Core.Repositories.Contracts;
using Speechless.Infrastructure.Repositories.LiteDB.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Speechless.Infrastructure.Repositories.LiteDB
{
    public class BusinessCardRepository : IBusinessCardRepository
    {
        private readonly IKeyGenerator<SequentialGuid> generator;
        private readonly LiteCollection<BusinessCard> cards;
        private const string KeyField = "_id";


        public BusinessCardRepository(IKeyGenerator<SequentialGuid> generator, LiteDatabase db)
        {
            this.generator = generator ?? throw new ArgumentNullException(nameof(generator));
            if (db is null) throw new ArgumentNullException(nameof(db));

            cards = db.GetCollection<BusinessCard>();
            cards.EnsureIndexes();
        }

        public bool ContainsKey(Guid key)
        {
            return cards.Exists(Query.EQ(KeyField, key));
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
            var count = cards.Count(Query.In(KeyField, keys.Select(key => new BsonValue(key))));
            return strict ? count != 0 && count == keys.Count() : count != 0;
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
            => cards.Delete(Query.In(KeyField, keys.Select(key => new BsonValue(key))));

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
            var tcs = new TaskCompletionSource<bool>();
            try
            {
                token.ThrowIfCancellationRequested();
                tcs.SetResult(EraseByKey(model.Id));
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

        public bool EraseByKey(Guid key) => cards.Delete(key);

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
            return offset != null && offset != null
                ? cards.Find(predicate, offset.Value, count.Value)
                : cards.Find(predicate);
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
        {
            return offset != null && offset != null
                ? cards.Find(Query.In(KeyField, keys.Select(key => new BsonValue(key))), offset.Value, count.Value)
                : cards.Find(Query.In(KeyField, keys.Select(key => new BsonValue(key))));
        }

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

        public BusinessCard FindByKey(Guid key, bool? references = null) => cards.FindById(key);

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
        {
            var matches = cards.FindAll();
            return offset != null && count != null
                ? matches.Skip(offset.Value).Take(offset.Value)
                : matches;
        }

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
            var matches = cards.FindAll().Select(x => x.Id);
            return offset != null && count != null
                ? matches.Skip(offset.Value).Take(offset.Value)
                : matches;
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

        public void Register(BusinessCard model, bool? references = null)
            => model.Id = generator.GetNext();

        public void RegisterAll(IEnumerable<BusinessCard> models, bool? references = null, int? offset = null, int? count = null)
        {
            foreach (var model in models)
                Register(model, references);
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
            var upserted = false;
            using (var scope = TransactionScopeOption.Required.CreateTransactionScope())
            {
                upserted = cards.Upsert(model);
                scope.Complete();
            }
            return upserted;
        }

        public int SaveAll(IEnumerable<BusinessCard> models, bool references = true)
        {
            var saved = 0;
            using (var scope = TransactionScopeOption.Required.CreateTransactionScope())
            {
                saved = cards.Upsert(models);
                scope.Complete();
            }
            return saved;
        }

        public Task<int> SaveAllAsync(IEnumerable<BusinessCard> models, bool references = true, CancellationToken token = default)
        {
            var exceptions = new List<Exception>();
            var saved = 0;
            try
            {
                using (var scope = TransactionScopeOption.Required.CreateTransactionScopeFlow())
                {
                    token.ThrowIfCancellationRequested();
                    saved = cards.Upsert(models);
                    scope.Complete();
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
            if (exceptions.Any()) throw new AggregateException(exceptions);
            return Task.FromResult(saved);
        }

        public Task<bool> SaveAsync(BusinessCard model, bool references = true, CancellationToken token = default)
        {
            var exceptions = new List<Exception>();
            var saved = false;
            try
            {
                using (var scope = TransactionScopeOption.Required.CreateTransactionScopeFlow())
                {
                    saved = cards.Upsert(model.Id, model);
                    scope.Complete();
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
            if (exceptions.Any()) throw new AggregateException(exceptions);
            return Task.FromResult(saved);
        }

        public void Trash(BusinessCard model, bool? references = null)
        {
            model.IsDeleted = true;
            Save(model, references ?? false);
        }

        public void TrashAll(IEnumerable<BusinessCard> models, bool? references = null)
        {
            foreach (var model in models) model.IsDeleted = true;
            SaveAll(models, references ?? false);
        }

        public Task TrashAllAsync(IEnumerable<BusinessCard> models, bool? references = null, CancellationToken token = default)
        {
            foreach (var model in models) model.IsDeleted = true;
            return SaveAllAsync(models, references ?? false, token);
        }

        public IEnumerable<BusinessCard> TrashAllByKeys(IEnumerable<Guid> keys, bool? references = null, int? offset = null, int? count = null)
        {
            var matches = FindAllByKeys(keys, references, offset, count);
            if (matches.Any()) TrashAll(matches, references);
            return matches;
        }

        public async Task<IEnumerable<BusinessCard>> TrashAllByKeysAsync(IEnumerable<Guid> keys, bool? references = null, int? offset = null, int? count = null, CancellationToken token = default)
        {
            var matches = await FindAllByKeysAsync(keys, references, offset, count, token).ConfigureAwait(false);
            if (matches.Any()) await TrashAllAsync(matches, references, token).ConfigureAwait(false);
            return matches;
        }

        public Task TrashAsync(BusinessCard model, bool? references = null)
        {
            model.IsDeleted = true;
            return SaveAsync(model, references ?? false);
        }

        public BusinessCard TrashByKey(Guid key, bool? references = null)
        {
            var match = FindByKey(key, references);
            if (match != null) Trash(match, references);
            return match;
        }

        public async Task<BusinessCard> TrashByKeyAsync(Guid key, bool? references = null, CancellationToken token = default)
        {
            var match = await FindByKeyAsync(key, references).ConfigureAwait(false);
            if (match != null) await TrashAsync(match, references).ConfigureAwait(false);
            return match;
        }

        public void Unregister(BusinessCard model, bool? references = null)
        {
            var exists = ContainsKey(model.Id);
            if (!exists) model.Id = generator.GetNullKey();
        }

        public void UnregisterAll(IEnumerable<BusinessCard> models, bool? references = null, int? offset = null, int? count = null)
        {
            var keys = models.Select(x => x.Id);
            var matches = FindAllByKeys(keys, references, offset, count);
            var nonmatches = models.Except(matches);
            foreach (var nonmatch in nonmatches)
                nonmatch.Id = generator.GetNullKey();
        }

        public async Task UnregisterAllAsync(IEnumerable<BusinessCard> models, bool? references = null, int? offset = null, int? count = null, CancellationToken cancellation = default)
        {
            try
            {
                var keys = models.Select(x => x.Id);
                var matches = await FindAllByKeysAsync(keys, references, offset, count, cancellation).ConfigureAwait(false);
                var nonmatches = models.Except(matches);
                foreach (var nonmatch in nonmatches)
                {
                    cancellation.ThrowIfCancellationRequested();
                    nonmatch.Id = generator.GetNullKey();
                }
            }
            catch (OperationCanceledException)
            {
                await Task.FromCanceled(cancellation);
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
            }
        }

        public async Task UnregisterAsync(BusinessCard model, bool? references = null, CancellationToken cancellation = default)
        {
            var exists = await ContainsKeyAsync(model.Id, cancellation).ConfigureAwait(false);
            if (!exists) model.Id = generator.GetNullKey();
        }
    }
}