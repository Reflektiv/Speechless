using Reflektiv.Speechless.Core.Domain.Concretes.Models;
using Speechless.Core.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Reflektiv.Speechless.Infrastucture.Repositories.EntityFramework
{
    public class BusinessCardRepository : IBusinessCardRepository
    {
        public bool ContainsKey(Guid key)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ContainsKeyAsync(Guid key, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKeys(IEnumerable<Guid> keys, bool strict = true)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ContainsKeysAsync(IEnumerable<Guid> keys, bool strict = true, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public bool Erase(BusinessCard model)
        {
            throw new NotImplementedException();
        }

        public int EraseAll(IEnumerable<BusinessCard> models)
        {
            throw new NotImplementedException();
        }

        public Task<int> EraseAllAsync(IEnumerable<BusinessCard> models, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public int EraseAllByKeys(IEnumerable<Guid> keys)
        {
            throw new NotImplementedException();
        }

        public Task<int> EraseAllByKeysAsync(IEnumerable<Guid> keys, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EraseAsync(BusinessCard model, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public bool EraseByKey(Guid key)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EraseByKeyAsync(Guid key, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public BusinessCard Find(Expression<Func<BusinessCard, bool>> predicate, bool references = true)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BusinessCard> FindAll(Expression<Func<BusinessCard, bool>> predicate, bool references = true, int? offset = null, int? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BusinessCard>> FindAllAsync(Expression<Func<BusinessCard, bool>> predicate, bool references = true, int? offset = null, int? count = null, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BusinessCard> FindAllByKeys(IEnumerable<Guid> keys, bool references = true, int? offset = null, int? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BusinessCard>> FindAllByKeysAsync(IEnumerable<Guid> keys, bool references = true, int? offset = null, int? count = null, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<BusinessCard> FindAsync(Expression<Func<BusinessCard, bool>> predicate, bool references = true, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public BusinessCard FindByKey(Guid key, bool references = true)
        {
            throw new NotImplementedException();
        }

        public Task<BusinessCard> FindByKeyAsync(Guid key, bool references = true, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BusinessCard> Get(bool references = true, int? offset = null, int? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BusinessCard>> GetAsync(bool references = true, int? offset = null, int? count = null, CancellationToken token = default)
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

        public void Register(BusinessCard model, bool references = false)
        {
            throw new NotImplementedException();
        }

        public void RegisterAll(IEnumerable<BusinessCard> models, bool references = false, int? offset = null, int? count = null)
        {
            throw new NotImplementedException();
        }

        public Task RegisterAllAsync(IEnumerable<BusinessCard> models, bool references = false, int? offset = null, int? count = null, CancellationToken cancellation = default)
        {
            throw new NotImplementedException();
        }

        public Task RegisterAsync(BusinessCard model, bool references = false, CancellationToken cancellation = default)
        {
            throw new NotImplementedException();
        }

        public void Restore(BusinessCard model, bool references = false)
        {
            throw new NotImplementedException();
        }

        public void RestoreAll(IEnumerable<BusinessCard> models, bool references = false)
        {
            throw new NotImplementedException();
        }

        public Task RestoreAllAsync(IEnumerable<BusinessCard> models, bool references = false, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BusinessCard> RestoreAllByKeys(IEnumerable<Guid> keys, bool references = false, int? offset = null, int? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BusinessCard>> RestoreAllByKeysAsync(IEnumerable<Guid> keys, bool references = false, int? offset = null, int? count = null, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task RestoreAsync(BusinessCard model, bool references = false)
        {
            throw new NotImplementedException();
        }

        public BusinessCard RestoreByKey(Guid key, bool references = false)
        {
            throw new NotImplementedException();
        }

        public Task<BusinessCard> RestoreByKeyAsync(Guid key, bool references = false)
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

        public void Trash(BusinessCard model, bool references = false)
        {
            throw new NotImplementedException();
        }

        public void TrashAll(IEnumerable<BusinessCard> models, bool references = false)
        {
            throw new NotImplementedException();
        }

        public Task TrashAllAsync(IEnumerable<BusinessCard> models, bool references = false, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BusinessCard> TrashAllByKeys(IEnumerable<Guid> keys, bool references = false, int? offset = null, int? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BusinessCard>> TrashAllByKeysAsync(IEnumerable<Guid> keys, bool references = false, int? offset = null, int? count = null, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task TrashAsync(BusinessCard model, bool references = false)
        {
            throw new NotImplementedException();
        }

        public BusinessCard TrashByKey(Guid key, bool references = false)
        {
            throw new NotImplementedException();
        }

        public Task<BusinessCard> TrashByKeyAsync(Guid key, bool references = false, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public void Unregister(BusinessCard model, bool references = false)
        {
            throw new NotImplementedException();
        }

        public void UnregisterAll(IEnumerable<BusinessCard> models, bool references = false, int? offset = null, int? count = null)
        {
            throw new NotImplementedException();
        }

        public Task UnregisterAllAsync(IEnumerable<BusinessCard> models, bool references = false, int? offset = null, int? count = null, CancellationToken cancellation = default)
        {
            throw new NotImplementedException();
        }

        public Task UnregisterAsync(BusinessCard model, bool references = false, CancellationToken cancellation = default)
        {
            throw new NotImplementedException();
        }
    }
}
