using reexmonkey.xmisc.core.linq.extensions;
using Reflektiv.Speechless.Infrastructure.Repositories.Tests.Fixtures;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Reflektiv.Speechless.Infrastructure.Repositories.Tests.Units
{
    [Collection(nameof(LiteDbCollectionFixture))]
    public class BusinessCardRepositoryAsyncTests
    {
        private readonly LiteDbFixture fixture;
        private readonly ITestOutputHelper console;

        public BusinessCardRepositoryAsyncTests(LiteDbFixture fixture, ITestOutputHelper console)
        {
            this.fixture = fixture;
            this.console = console;
        }

        [Fact]
        public async Task ShouldContainKeyAsync()
        {
            //arrange
            fixture.ResetDatastore();
            var card = fixture.GenerateBusinessCard();
            await fixture.Repository.RegisterAsync(card);
            await fixture.Repository.SaveAsync(card);

            //act
            var result = await fixture.Repository.ContainsKeyAsync(card.Id);

            //assert
            Assert.True(result);
        }

        [Fact]
        public async Task ShouldContainKeysAsync()
        {
            //arrange
            fixture.ResetDatastore();
            var cards = fixture.GenerateBusinessCards();
            await fixture.Repository.RegisterAllAsync(cards);
            await fixture.Repository.SaveAllAsync(cards);
            var keys = cards.Select(card => card.Id);
            //act
            var result = await fixture.Repository.ContainsKeysAsync(keys);

            //assert
            Assert.True(result);
        }

        [Fact]
        public async Task ShouldEraseBusinessCardAsync()
        {
            //arrange
            fixture.ResetDatastore();
            var card = fixture.GenerateBusinessCard();
            await fixture.Repository.RegisterAsync(card);
            await fixture.Repository.SaveAsync(card);

            //act
            var result = await fixture.Repository.EraseAsync(card);

            //assert
            Assert.True(result);
            Assert.False(await fixture.Repository.ContainsKeyAsync(card.Id));
        }

        [Fact]
        public async Task ShouldEraseBusinessCardsAsync()
        {
            //arrange
            fixture.ResetDatastore();
            var cards = fixture.GenerateBusinessCards();
            await fixture.Repository.RegisterAllAsync(cards);
            await fixture.Repository.SaveAllAsync(cards);
            var keys = cards.Select(card => card.Id);

            //act
            var result = await fixture.Repository.EraseAllAsync(cards);

            //assert
            Assert.Equal(cards.Count, result);
            Assert.False(await fixture.Repository.ContainsKeysAsync(keys));
        }

        [Fact]
        public async Task ShouldEraseBusinessCardByIdAsync()
        {
            //arrange
            fixture.ResetDatastore();
            var card = fixture.GenerateBusinessCard();
            await fixture.Repository.RegisterAsync(card);
            await fixture.Repository.SaveAsync(card);

            //act
            var result = await fixture.Repository.EraseByKeyAsync(card.Id);

            //assert
            Assert.True(result);
            Assert.False(await fixture.Repository.ContainsKeyAsync(card.Id));
        }

        [Fact]
        public async Task ShouldEraseBusinessCardsByIds()
        {
            //arrange
            fixture.ResetDatastore();
            var cards = fixture.GenerateBusinessCards();
            await fixture.Repository.RegisterAllAsync(cards);
            await fixture.Repository.SaveAllAsync(cards);
            var keys = cards.Select(card => card.Id);

            //act
            var results = fixture.Repository.EraseAllByKeys(keys);

            //assert
            Assert.Equal(cards.Count, results);
            Assert.False(await fixture.Repository.ContainsKeysAsync(keys));
        }

        [Fact]
        public async Task ShouldFindBusinessCardsAsync()
        {
            //arrange
            fixture.ResetDatastore();
            var cards = fixture.GenerateBusinessCards();
            var today = DateTime.Now;
            cards[0].BirthDay = today;
            cards[1].BirthDay = today;
            await fixture.Repository.RegisterAllAsync(cards);
            await fixture.Repository.SaveAllAsync(cards);
            var selected = cards.Where(x => x.BirthDay != null);

            //act
            var results = await fixture.Repository.FindAllAsync(x => x.BirthDay != null);

            //assert
            Assert.NotEmpty(results);
            Assert.True(results.SequenceEqual(selected, x => x.Id));
        }

        [Fact]
        public async Task ShouldFindBusinessCardsByKeysAsync()
        {
            //arrange
            fixture.ResetDatastore();
            var cards = fixture.GenerateBusinessCards();
            var today = DateTime.Now;
            cards[0].BirthDay = today;
            cards[1].BirthDay = today;
            await fixture.Repository.RegisterAllAsync(cards);
            await fixture.Repository.SaveAllAsync(cards);
            var keys = cards.Where(x => x.BirthDay != null).Select(x => x.Id);

            //act
            var results = await fixture.Repository.FindAllByKeysAsync(keys);

            //assert
            Assert.NotEmpty(results);
            Assert.True(keys.SequenceEqual(results.Select(x => x.Id), x => x));
        }

        [Fact]
        public async Task ShouldRetrieveBusinessCardsAsync()
        {
            //arrange
            fixture.ResetDatastore();
            var cards = fixture.GenerateBusinessCards();
            await fixture.Repository.RegisterAllAsync(cards);
            await fixture.Repository.SaveAllAsync(cards);

            //act
            var results = await fixture.Repository.GetAsync();

            //assert
            Assert.NotEmpty(results);
            Assert.True(results.SequenceEqual(cards, x => x.Id));
        }

        [Fact]
        public async Task ShouldRetrieveBusinessCardKeysAsync()
        {
            //arrange
            fixture.ResetDatastore();
            var cards = fixture.GenerateBusinessCards();
            await fixture.Repository.RegisterAllAsync(cards);
            await fixture.Repository.SaveAllAsync(cards);
            var keys = cards.Select(x => x.Id);

            //act
            var results = await fixture.Repository.GetKeysAsync();

            //assert
            Assert.NotEmpty(results);
            Assert.True(keys.SequenceEqual(results, x => x));
        }

        [Fact]
        public async Task ShouldRegisterBusinessCardAsync()
        {
            //arrange
            fixture.ResetDatastore();
            var card = fixture.GenerateBusinessCard();

            //act
            await fixture.Repository.RegisterAsync(card);

            //assert
            Assert.NotEqual(Guid.Empty, card.Id);
        }

        [Fact]
        public async Task ShouldRegisterBusinessCardsAsync()
        {
            //arrange
            fixture.ResetDatastore();
            var cards = fixture.GenerateBusinessCards();

            //act
            await fixture.Repository.RegisterAllAsync(cards);

            //assert
            var keys = cards.Where(x => x.Id != Guid.Empty).Select(x => x.Id);
            Assert.NotEmpty(keys);
            Assert.Equal(cards.Count, keys.Count());
        }

        [Fact]
        public async Task ShouldRestoreBusinessCardAsync()
        {
            //arrange
            fixture.ResetDatastore();
            var card = fixture.GenerateBusinessCard();
            await fixture.Repository.RegisterAsync(card);
            await fixture.Repository.TrashAsync(card);

            //act
            await fixture.Repository.RestoreAsync(card);

            //assert
            var match = await fixture.Repository.FindByKeyAsync(card.Id);
            Assert.NotNull(match);
            Assert.False(match.IsDeleted);
        }

        [Fact]
        public async Task ShouldRestoreBusinessCardsAsync()
        {
            //arrange
            fixture.ResetDatastore();
            var cards = fixture.GenerateBusinessCards();
            await fixture.Repository.RegisterAllAsync(cards);
            await fixture.Repository.TrashAllAsync(cards);
            var keys = cards.Select(card => card.Id);

            //act
            await fixture.Repository.RestoreAllAsync(cards);

            //assert
            var matches = await fixture.Repository.FindAllByKeysAsync(keys);
            Assert.NotEmpty(matches);
            Assert.True(matches.All(card => !card.IsDeleted));
        }

        [Fact]
        public async Task ShouldPersistBusinessCardAsync()
        {
            //arrange
            fixture.ResetDatastore();
            var card = fixture.GenerateBusinessCard();
            await fixture.Repository.RegisterAsync(card);

            //act
            await fixture.Repository.SaveAsync(card);

            //assert
            var match = await fixture.Repository.FindByKeyAsync(card.Id);
            Assert.NotNull(match);
            Assert.Equal(card, match);
        }

        [Fact]
        public async Task ShouldPersistBusinessCardsAsync()
        {
            //arrange
            fixture.ResetDatastore();
            var cards = fixture.GenerateBusinessCards();
            await fixture.Repository.RegisterAllAsync(cards);
            var keys = cards.Select(x => x.Id);

            //act
            await fixture.Repository.SaveAllAsync(cards);

            //assert
            var matches = await fixture.Repository.FindAllByKeysAsync(keys);
            Assert.NotEmpty(matches);
            Assert.True(cards.SequenceEqual(matches, x => x.Id));
        }

        [Fact]
        public async Task ShouldTrashBusinessCardAsync()
        {
            //arrange
            fixture.ResetDatastore();
            var card = fixture.GenerateBusinessCard();
            await fixture.Repository.RegisterAsync(card);

            //act
            await fixture.Repository.TrashAsync(card);

            //assert
            var match = await fixture.Repository.FindByKeyAsync(card.Id);
            Assert.NotNull(match);
            Assert.True(match.IsDeleted);
        }

        [Fact]
        public async Task ShouldTrashBusinessCardsAsync()
        {
            //arrange
            fixture.ResetDatastore();
            var cards = fixture.GenerateBusinessCards();
            await fixture.Repository.RegisterAllAsync(cards);
            var keys = cards.Select(x => x.Id);

            //act
            await fixture.Repository.TrashAllAsync(cards);

            //assert
            var matches = await fixture.Repository.FindAllByKeysAsync(keys);
            Assert.NotEmpty(matches);
            Assert.True(matches.All(card => card.IsDeleted));
        }

        [Fact]
        public async Task ShouldUnregisterBusinessCardAsync()
        {
            //arrange
            fixture.ResetDatastore();
            var card = fixture.GenerateBusinessCard();
            await fixture.Repository.RegisterAsync(card);

            //act
            await fixture.Repository.UnregisterAsync(card);

            //assert
            Assert.Equal(Guid.Empty, card.Id);
        }

        [Fact]
        public async Task ShouldUnregsterBusinessCardsAsync()
        {
            //arrange
            fixture.ResetDatastore();
            var cards = fixture.GenerateBusinessCards();
            await fixture.Repository.RegisterAllAsync(cards);

            //act
            await fixture.Repository.UnregisterAllAsync(cards);

            //assert
            Assert.True(cards.All(x => x.Id == Guid.Empty));
        }
    }
}