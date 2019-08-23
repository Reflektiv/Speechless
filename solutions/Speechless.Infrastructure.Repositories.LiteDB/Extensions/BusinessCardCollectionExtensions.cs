using LiteDB;
using Reflektiv.Speechless.Core.Domain.Concretes.Models;

namespace Speechless.Infrastructure.Repositories.LiteDB.Extensions
{
    public static class BusinessCardCollectionExtensions
    {
        public static void EnsureIndexes(this LiteCollection<BusinessCard> collection)
        {
            collection.EnsureIndex(x => x.Id, true);
            collection.EnsureIndex(x => x.FirstName);
            collection.EnsureIndex(x => x.LastName);
            collection.EnsureIndex(x => x.BirthDay);
        }

        public static void DropIndexes(this LiteCollection<BusinessCard> collection)
        {
            collection.DropIndex(nameof(BusinessCard.Id));
            collection.DropIndex(nameof(BusinessCard.FirstName));
            collection.DropIndex(nameof(BusinessCard.LastName));
            collection.DropIndex(nameof(BusinessCard.BirthDay));
        }
    }
}