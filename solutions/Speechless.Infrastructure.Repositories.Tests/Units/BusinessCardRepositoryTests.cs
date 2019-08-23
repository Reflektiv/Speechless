using reexmonkey.xmisc.core.linq.extensions;
using Reflektiv.Speechless.Infrastructure.Repositories.Tests.Fixtures;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Reflektiv.Speechless.Infrastructure.Repositories.Tests.Units
{
    [Collection(nameof(LiteDbCollectionFixture))]
    public class BusinessCardRepositoryTests
    {
        private readonly LiteDbFixture fixture;
        private readonly ITestOutputHelper console;

        public BusinessCardRepositoryTests(LiteDbFixture fixture, ITestOutputHelper console)
        {
            this.fixture = fixture;
            this.console = console;
        }

        [Fact]
        public void ShouldContainKey()
        {
            //arrange
            fixture.ResetDatastore();
            var card = fixture.GenerateBusinessCard();
            fixture.Repository.Register(card);
            fixture.Repository.Save(card);

            //act
            var result = fixture.Repository.ContainsKey(card.Id);

            //assert
            Assert.True(result);
        }

        [Fact]
        public void ShouldContainKeys()
        {
            //arrange
            fixture.ResetDatastore();
            var cards = fixture.GenerateBusinessCards();
            fixture.Repository.RegisterAll(cards);
            fixture.Repository.SaveAll(cards);
            var keys = cards.Select(card => card.Id);
            //act
            var result = fixture.Repository.ContainsKeys(keys);

            //assert
            Assert.True(result);
        }

        [Fact]
        public void ShouldEraseBusinessCard()
        {
            //arrange
            fixture.ResetDatastore();
            var card = fixture.GenerateBusinessCard();
            fixture.Repository.Register(card);
            fixture.Repository.Save(card);

            //act
            var result = fixture.Repository.Erase(card);

            //assert
            Assert.True(result);
            Assert.False(fixture.Repository.ContainsKey(card.Id));
        }

        [Fact]
        public void ShouldEraseBusinessCards()
        {
            //arrange
            fixture.ResetDatastore();
            var cards = fixture.GenerateBusinessCards();
            fixture.Repository.RegisterAll(cards);
            fixture.Repository.SaveAll(cards);
            var keys = cards.Select(card => card.Id);

            //act
            var result = fixture.Repository.EraseAll(cards);

            //assert
            Assert.Equal(cards.Count, result);
            Assert.False(fixture.Repository.ContainsKeys(keys));
        }

        [Fact]
        public void ShouldEraseBusinessCardById()
        {
            //arrange
            fixture.ResetDatastore();
            var card = fixture.GenerateBusinessCard();
            fixture.Repository.Register(card);
            fixture.Repository.Save(card);

            //act
            var result = fixture.Repository.EraseByKey(card.Id);

            //assert
            Assert.True(result);
            Assert.False(fixture.Repository.ContainsKey(card.Id));
        }

        [Fact]
        public void ShouldEraseBusinessCardsByIds()
        {
            //arrange
            fixture.ResetDatastore();
            var cards = fixture.GenerateBusinessCards();
            fixture.Repository.RegisterAll(cards);
            fixture.Repository.SaveAll(cards);
            var keys = cards.Select(card => card.Id);

            //act
            var results = fixture.Repository.EraseAllByKeys(keys);

            //assert
            Assert.Equal(cards.Count, results);
            Assert.False(fixture.Repository.ContainsKeys(keys));
        }

        [Fact]
        public void ShouldFindBusinessCards()
        {
            //arrange
            fixture.ResetDatastore();
            var cards = fixture.GenerateBusinessCards();
            var today = DateTime.Now;
            cards[0].BirthDay = today;
            cards[1].BirthDay = today;
            fixture.Repository.RegisterAll(cards);
            fixture.Repository.SaveAll(cards);
            var selected = cards.Where(x => x.BirthDay != null);

            //act
            var results = fixture.Repository.FindAll(x => x.BirthDay != null);

            //assert
            Assert.NotEmpty(results);
            Assert.True(results.SequenceEqual(selected, x => x.Id));
        }

        [Fact]
        public void ShouldFindBusinessCardsByKeys()
        {
            //arrange
            fixture.ResetDatastore();
            var cards = fixture.GenerateBusinessCards();
            var today = DateTime.Now;
            cards[0].BirthDay = today;
            cards[1].BirthDay = today;
            fixture.Repository.RegisterAll(cards);
            fixture.Repository.SaveAll(cards);
            var keys = cards.Where(x => x.BirthDay != null).Select(x => x.Id);

            //act
            var results = fixture.Repository.FindAllByKeys(keys);

            //assert
            Assert.NotEmpty(results);
            Assert.True(keys.SequenceEqual(results.Select(x => x.Id), x => x));
        }

        [Fact]
        public void ShouldRetrieveBusinessCards()
        {
            //arrange
            fixture.ResetDatastore();
            var cards = fixture.GenerateBusinessCards();
            fixture.Repository.RegisterAll(cards);
            fixture.Repository.SaveAll(cards);

            //act
            var results = fixture.Repository.Get();

            //assert
            Assert.NotEmpty(results);
            Assert.True(results.SequenceEqual(cards, x => x.Id));
        }

        [Fact]
        public void ShouldRetrieveBusinessCardKeys()
        {
            //arrange
            fixture.ResetDatastore();
            var cards = fixture.GenerateBusinessCards();
            fixture.Repository.RegisterAll(cards);
            fixture.Repository.SaveAll(cards);
            var keys = cards.Select(x => x.Id);

            //act
            var results = fixture.Repository.GetKeys();

            //assert
            Assert.NotEmpty(results);
            Assert.True(keys.SequenceEqual(results, x => x));
        }

        [Fact]
        public void ShouldRegisterBusinessCard()
        {
            //arrange
            fixture.ResetDatastore();
            var card = fixture.GenerateBusinessCard();

            //act
            fixture.Repository.Register(card);

            //assert
            Assert.NotEqual(Guid.Empty, card.Id);
        }

        [Fact]
        public void ShouldRegisterBusinessCards()
        {
            //arrange
            fixture.ResetDatastore();
            var cards = fixture.GenerateBusinessCards();

            //act
            fixture.Repository.RegisterAll(cards);

            //assert
            var keys = cards.Where(x => x.Id != Guid.Empty).Select(x => x.Id);
            Assert.NotEmpty(keys);
            Assert.Equal(cards.Count, keys.Count());
        }

        [Fact]
        public void ShouldRestoreBusinessCard()
        {
            //arrange
            fixture.ResetDatastore();
            var card = fixture.GenerateBusinessCard();
            fixture.Repository.Register(card);
            fixture.Repository.Trash(card);

            //act
            fixture.Repository.Restore(card);

            //assert
            var match = fixture.Repository.FindByKey(card.Id);
            Assert.NotNull(match);
            Assert.False(match.IsDeleted);
        }

        [Fact]
        public void ShouldRestoreBusinessCards()
        {
            //arrange
            fixture.ResetDatastore();
            var cards = fixture.GenerateBusinessCards();
            fixture.Repository.RegisterAll(cards);
            fixture.Repository.TrashAll(cards);
            var keys = cards.Select(card => card.Id);

            //act
            fixture.Repository.RestoreAll(cards);

            //assert
            var matches = fixture.Repository.FindAllByKeys(keys);
            Assert.NotEmpty(matches);
            Assert.True(matches.All(card => !card.IsDeleted));
        }

        [Fact]
        public void ShouldPersistBusinessCard()
        {
            //arrange
            fixture.ResetDatastore();
            var card = fixture.GenerateBusinessCard();
            fixture.Repository.Register(card);

            //act
            fixture.Repository.Save(card);

            //assert
            var match = fixture.Repository.FindByKey(card.Id);
            Assert.NotNull(match);
            Assert.Equal(card, match);
        }

        [Fact]
        public void ShouldPersistBusinessCards()
        {
            //arrange
            fixture.ResetDatastore();
            var cards = fixture.GenerateBusinessCards();
            fixture.Repository.RegisterAll(cards);
            var keys = cards.Select(x => x.Id);

            //act
            fixture.Repository.SaveAll(cards);

            //assert
            var matches = fixture.Repository.FindAllByKeys(keys);
            Assert.NotEmpty(matches);
            Assert.True(cards.SequenceEqual(matches, x => x.Id));
        }

        [Fact]
        public void ShouldTrashBusinessCard()
        {
            //arrange
            fixture.ResetDatastore();
            var card = fixture.GenerateBusinessCard();
            fixture.Repository.Register(card);

            //act
            fixture.Repository.Trash(card);

            //assert
            var match = fixture.Repository.FindByKey(card.Id);
            Assert.NotNull(match);
            Assert.True(match.IsDeleted);
        }

        [Fact]
        public void ShouldTrashBusinessCards()
        {
            //arrange
            fixture.ResetDatastore();
            var cards = fixture.GenerateBusinessCards();
            fixture.Repository.RegisterAll(cards);
            var keys = cards.Select(x => x.Id);

            //act
            fixture.Repository.TrashAll(cards);

            //assert
            var matches = fixture.Repository.FindAllByKeys(keys);
            Assert.NotEmpty(matches);
            Assert.True(matches.All(card => card.IsDeleted));
        }

        [Fact]
        public void ShouldUnregisterBusinessCard()
        {
            //arrange
            fixture.ResetDatastore();
            var card = fixture.GenerateBusinessCard();
            fixture.Repository.Register(card);

            //act
            fixture.Repository.Unregister(card);

            //assert
            Assert.Equal(Guid.Empty, card.Id);
        }

        [Fact]
        public void ShouldUnregsterBusinessCards()
        {
            //arrange
            fixture.ResetDatastore();
            var cards = fixture.GenerateBusinessCards();
            fixture.Repository.RegisterAll(cards);

            //act
            fixture.Repository.UnregisterAll(cards);

            //assert
            Assert.True(cards.All(x => x.Id == Guid.Empty));
        }
    }
}