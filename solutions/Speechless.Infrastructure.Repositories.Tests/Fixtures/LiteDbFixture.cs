using LiteDB;
using reexmonkey.xmisc.backbone.identifiers.concretes.models;
using reexmonkey.xmisc.backbone.identifiers.contracts.models;
using Speechless.Core.Repositories.Contracts;
using Speechless.Infrastructure.Repositories.LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Reflektiv.Speechless.Infrastructure.Repositories.Tests.Fixtures
{
    public class LiteDbFixture : FixtureBase, IDisposable
    {
        private readonly LiteDatabase database;
        private readonly IKeyGenerator<SequentialGuid> generator;

        public IBusinessCardRepository Repository { get; }

        public LiteDbFixture()
        {
            var connection = $"{Path.Combine(GetDataDirectory().FullName, "test.db")}";
            database = new LiteDatabase(connection);
            generator = new SequentialGuidKeyGenerator();
            Repository = new BusinessCardRepository(generator, database);
        }

        public void ResetDatastore()
        {
            var collections = database.GetCollectionNames().ToList();
            for (int i = 0; i < collections.Count; i++)
            {
                database.DropCollection(collections[i]);
            }
        }

        public void Dispose()
        {
            if (database != null) database.Dispose();
        }
    }
}
