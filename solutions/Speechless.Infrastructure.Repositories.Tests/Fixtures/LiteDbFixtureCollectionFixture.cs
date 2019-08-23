using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Reflektiv.Speechless.Infrastructure.Repositories.Tests.Fixtures
{
    [CollectionDefinition(nameof(LiteDbCollectionFixture))]
    public class LiteDbCollectionFixture: ICollectionFixture<LiteDbFixture>
    {

    }
}
