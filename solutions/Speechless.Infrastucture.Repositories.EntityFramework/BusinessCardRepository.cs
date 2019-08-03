using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using PwC.xEric.Infrastructure.Repositories.Ormlite.Extensions;
using reexmonkey.xmisc.backbone.identifiers.contracts.models;
using Reflektiv.Speechless.Core.Domain.Concretes.Models;
using Speechless.Core.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Reflektiv.Speechless.Infrastucture.Repositories.EntityFramework
{
    public class BusinessCardRepository : IBusinessCardRepository
    {
        private readonly IKeyGenerator<SequentialGuid> generator;

        public BusinessCardRepository(IKeyGenerator<SequentialGuid> generator)
        {
            this.generator = generator ?? throw new ArgumentNullException(nameof(generator));
        }

        public bool ContainsKey(Guid key)
        {
            using (var context = new RepositoryContext())
            {
                return context.BusinessCards.Count(x => x.Id == key) != 0;
            }
        }

        public async Task<bool> ContainsKeyAsync(Guid key, CancellationToken token = default)
        {
            using (var context = new RepositoryContext())
            {
                return await context.BusinessCards.CountAsync(x => x.Id == key, token).ConfigureAwait(false) != 0;
            }
        }

        public bool ContainsKeys(IEnumerable<Guid> keys, bool strict = true)
        {
            using (var context = new RepositoryContext())
            {
                var count = context.BusinessCards.Count(x => keys.Contains(x.Id));
                return strict ? count != 0 && count == keys.Count() : count != 0;
            }
        }

        public async Task<bool> ContainsKeysAsync(IEnumerable<Guid> keys, bool strict = true, CancellationToken token = default)
        {
            using (var context = new RepositoryContext())
            {
                var count = await context.BusinessCards.CountAsync(x => keys.Contains(x.Id), token).ConfigureAwait(false);
                return strict ? count != 0 && count == keys.Count() : count != 0;
            }
        }

        public bool Erase(BusinessCard model) => EraseByKey(model.Id);

        public int EraseAll(IEnumerable<BusinessCard> models) => EraseAllByKeys(models.Select(x => x.Id));

        public Task<int> EraseAllAsync(IEnumerable<BusinessCard> models, CancellationToken token = default)
            => EraseAllByKeysAsync(models.Select(x => x.Id), token);

        public int EraseAllByKeys(IEnumerable<Guid> keys)
        {
            var deletes = 0;
            using (var scope = EfCoreExtensions.CreateTransactionScope(TransactionScopeOption.Required))
            {
                using (var context = new RepositoryContext())
                {
                    deletes = context.BusinessCards.Where(x => keys.Contains(x.Id)).BatchDelete();
                    context.SaveChanges();
                }
                scope.Complete();
            }
            return deletes;
        }

        public async Task<int> EraseAllByKeysAsync(IEnumerable<Guid> keys, CancellationToken token = default)
        {
            var result = 0;
            using (var scope = EfCoreExtensions.CreateTransactionScopeFlow(TransactionScopeOption.Required))
            {
                using (var context = new RepositoryContext())
                {
                    result = await context.BusinessCards.Where(x => keys.Contains(x.Id)).BatchDeleteAsync().ConfigureAwait(false);
                    await context.SaveChangesAsync(token).ConfigureAwait(false);
                }
                scope.Complete();
            }
            return result;
        }

        public Task<bool> EraseAsync(BusinessCard model, CancellationToken token = default) => EraseByKeyAsync(model.Id, token);

        public bool EraseByKey(Guid key)
        {
            int deletes = 0;
            using (var scope = EfCoreExtensions.CreateTransactionScopeFlow(TransactionScopeOption.Required))
            {
                using (var context = new RepositoryContext())
                {
                    deletes = context.BusinessCards.Where(x => x.Id == key).BatchDelete();
                    context.SaveChanges();
                }
                scope.Complete();
            }
            return deletes != 0;
        }

        public async Task<bool> EraseByKeyAsync(Guid key, CancellationToken token = default)
        {
            int deletes = 0;
            using (var scope = EfCoreExtensions.CreateTransactionScopeFlow(TransactionScopeOption.Required))
            {
                using (var context = new RepositoryContext())
                {
                    deletes = await context.BusinessCards.Where(x => x.Id == key).BatchDeleteAsync();
                    await context.SaveChangesAsync(token).ConfigureAwait(false);
                }
                scope.Complete();
            }
            return deletes != 0;
        }

        public IEnumerable<BusinessCard> FindAll(Expression<Func<BusinessCard, bool>> predicate, bool? references = null, int? offset = null, int? count = null)
        {
            using (var context = new RepositoryContext())
            {
                var matches = context.BusinessCards.Where(predicate).ToList();
                return offset != null && count != null
                    ? matches.Skip(offset.Value).Take(count.Value)
                    : matches;
            }
        }

        public async Task<IEnumerable<BusinessCard>> FindAllAsync(Expression<Func<BusinessCard, bool>> predicate, bool? references = null, int? offset = null, int? count = null, CancellationToken token = default)
        {
            {
                using (var context = new RepositoryContext())
                {
                    var matches = await context.BusinessCards.Where(predicate).ToListAsync(token).ConfigureAwait(false);
                    return offset != null && count != null
                        ? matches.Skip(offset.Value).Take(count.Value)
                        : matches;
                }
            }
        }

        public IEnumerable<BusinessCard> FindAllByKeys(IEnumerable<Guid> keys, bool? references = null, int? offset = null, int? count = null)
        {
            using (var context = new RepositoryContext())
            {
                var matches = context.BusinessCards.Where(x => keys.Contains(x.Id)).ToList();
                return offset != null && count != null
                    ? matches.Skip(offset.Value).Take(count.Value)
                    : matches;
            }
        }

        public async Task<IEnumerable<BusinessCard>> FindAllByKeysAsync(IEnumerable<Guid> keys, bool? references = null, int? offset = null, int? count = null, CancellationToken token = default)
        {
            using (var context = new RepositoryContext())
            {
                var matches = await context.BusinessCards.Where(x => keys.Contains(x.Id)).ToListAsync();
                return offset != null && count != null
                    ? matches.Skip(offset.Value).Take(count.Value)
                    : matches;
            }
        }

        public async Task<BusinessCard> FindAsync(Expression<Func<BusinessCard, bool>> predicate, bool? references = null, CancellationToken token = default)
        {
            using (var context = new RepositoryContext())
            {
                return await context.BusinessCards.Where(predicate).FirstOrDefaultAsync(token).ConfigureAwait(false);
            }
        }

        public BusinessCard FindByKey(Guid key, bool? references = null)
        {
            using (var context = new RepositoryContext())
            {
                return context.BusinessCards.Find(key);
            }
        }

        public async Task<BusinessCard> FindByKeyAsync(Guid key, bool? references = null, CancellationToken token = default)
        {
            using (var context = new RepositoryContext())
            {
                return await context.BusinessCards.FindAsync(key, token).ConfigureAwait(false);
            }
        }

        public IEnumerable<BusinessCard> Get(bool? references = null, int? offset = null, int? count = null)
        {
            using (var context = new RepositoryContext())
            {
                var matches = context.BusinessCards.ToList();
                return offset != null && count != null
                    ? matches.Skip(offset.Value).Take(count.Value)
                    : matches;
            }
        }

        public async Task<IEnumerable<BusinessCard>> GetAsync(bool? references = null, int? offset = null, int? count = null, CancellationToken token = default)
        {
            using (var context = new RepositoryContext())
            {
                var matches = await context.BusinessCards.ToListAsync();
                return offset != null && count != null
                    ? matches.Skip(offset.Value).Take(count.Value)
                    : matches;
            }
        }

        public IEnumerable<Guid> GetKeys(int? offset = null, int? count = null)
        {
            using (var context = new RepositoryContext())
            {
                var matches = context.BusinessCards.Select(x => x.Id).ToList();
                return offset != null && count != null
                    ? matches.Skip(offset.Value).Take(count.Value)
                    : matches;
            }
        }

        public async Task<IEnumerable<Guid>> GetKeysAsync(int? offset = null, int? count = null, CancellationToken token = default)
        {
            using (var context = new RepositoryContext())
            {
                var matches = await context.BusinessCards.Select(x => x.Id).ToListAsync(token).ConfigureAwait(false);
                return offset != null && count != null
                    ? matches.Skip(offset.Value).Take(count.Value)
                    : matches;
            }
        }

        public void Register(BusinessCard model, bool? references = null)
        {
            FindAll(card => card.FirstName == model.FirstName || card.LastName != model.LastName);
            model.Id = generator.GetNext();
        }

        public void RegisterAll(IEnumerable<BusinessCard> models, bool? references = null, int? offset = null, int? count = null)
        {
            foreach (var model in models) Register(model, references);
        }

        public Task RegisterAllAsync(IEnumerable<BusinessCard> models, bool? references = null, int? offset = null, int? count = null, CancellationToken cancellation = default)
        {
            RegisterAll(models, references);
            return Task.CompletedTask;
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
            var inserted = false;
            using (var scope = EfCoreExtensions.CreateTransactionScope(TransactionScopeOption.Required))
            {
                using (var context = new RepositoryContext())
                {
                    var match = context.BusinessCards.Find(model.Id);
                    if (match == null) context.BusinessCards.Add(model);
                    else context.BusinessCards.Update(model);
                    inserted = context.SaveChanges() != 0;
                }
                scope.Complete();
            }
            return inserted;
        }

        public int SaveAll(IEnumerable<BusinessCard> models, bool references = true)
        {
            var inserts = 0;
            using (var scope = EfCoreExtensions.CreateTransactionScope(TransactionScopeOption.Required))
            {
                using (var context = new RepositoryContext())
                {
                    context.BulkInsertOrUpdate(models as IList<BusinessCard> ?? models.ToList());
                    inserts = context.SaveChanges();
                }
                scope.Complete();
            }
            return inserts;
        }

        public async Task<int> SaveAllAsync(IEnumerable<BusinessCard> models, bool references = true, CancellationToken token = default)
        {
            var inserts = 0;
            using (var scope = EfCoreExtensions.CreateTransactionScope(TransactionScopeOption.Required))
            {
                using (var context = new RepositoryContext())
                {
                    await context.BulkInsertOrUpdateAsync(models as IList<BusinessCard> ?? models.ToList()).ConfigureAwait(false);
                    inserts = await context.SaveChangesAsync(token).ConfigureAwait(false);
                }
                scope.Complete();
            }
            return inserts;
        }

        public async Task<bool> SaveAsync(BusinessCard model, bool references = true, CancellationToken token = default)
        {
            var inserted = false;
            using (var scope = EfCoreExtensions.CreateTransactionScope(TransactionScopeOption.Required))
            {
                using (var context = new RepositoryContext())
                {
                    var match = await context.BusinessCards.FindAsync(model.Id).ConfigureAwait(false);
                    if (match == null) await context.BusinessCards.AddAsync(model).ConfigureAwait(false);
                    else context.BusinessCards.Update(model);
                    inserted = await context.SaveChangesAsync(token).ConfigureAwait(false) != 0;
                }
                scope.Complete();
            }
            return inserted;
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
            foreach (var nonmatch in nonmatches) nonmatch.Id = generator.GetNullKey();
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