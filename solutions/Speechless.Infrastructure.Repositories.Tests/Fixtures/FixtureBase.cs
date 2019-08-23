using MixERP.Net.VCards.Models;
using MixERP.Net.VCards.Types;
using reexmonkey.xmisc.core.reflection.extensions;
using Reflektiv.Speechless.Core.Domain.Concretes.Models;
using Reflektiv.Speechless.Infrastructure.Repositories.Tests.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Reflektiv.Speechless.Infrastructure.Repositories.Tests.Fixtures
{
    public abstract class FixtureBase
    {
        protected const string DATA = "data";
        protected const string OUTPUT = "Output";
        protected const string PHOTO = "open-xamarin.png";

        protected DirectoryInfo GetDirectory(DirectoryInfo root, string path)
        {
            var dir = root != null && root.Exists
                ? Path.Combine(root.FullName, path)
                : path;

            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            return new DirectoryInfo(dir);
        }

        protected DirectoryInfo GetBinDirectory()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetFilePath());
            return Directory.CreateDirectory(path);
        }

        protected DirectoryInfo GetProjectDirectory()
            => GetBinDirectory()?.Parent?.Parent?.Parent;

        protected DirectoryInfo GetDataDirectory()
            => GetDirectory(GetProjectDirectory(), DATA);

        protected DirectoryInfo GetOutputDirectory()
            => GetDirectory(GetProjectDirectory(), OUTPUT);

        public Uri GenerateUri()
        {
            return new Uri ($"https://{Faker.Internet.DomainName()}.{Faker.Internet.DomainSuffix()}", UriKind.RelativeOrAbsolute);
        }

        public Impp GenerateImpp()
        {
            return new Impp
            {
                Address = GenerateUri(),
                Preference = Faker.RandomNumber.Next(1, 10)
            };
        }

        public Language GenerateLanguage()
        {
            return new Language
            {
                Name = Faker.ISOCountryCode.Next(),
                Type = (LanguageType)Faker.RandomNumber.Next(0, 2)
            };
        }

        public Relation GenerateRelation()
        {
            var type = (RelationType)Faker.RandomNumber.Next(0, 19);
            return new Relation
            {
                RelationUri = new Uri($"{GenerateUri()}/relation/{type.ToString("G")}"),
                Type = type,
                Preference = Faker.RandomNumber.Next(1, 10)
            };
        }

        public Address GenerateAddress()
        {
            return new Address
            {
                Street = Faker.Address.StreetAddress(),
                PostalCode = Faker.Address.UkPostCode(),
                Locality = Faker.Address.City(),
                Region = Faker.Address.UkCounty(),
                Country = Faker.Address.Country(),
                Preference = Faker.RandomNumber.Next(1, 10)
            };
        }

        public DeliveryAddress GenerateDeliveryAddress()
        {
            return new DeliveryAddress
            {
                Address = GenerateAddress().AsString(),
                Type = (AddressType)Faker.RandomNumber.Next(0, 5)
            };
        }

        public Telephone GenerateTelephone()
        {
            return new Telephone
            {
                Number = Faker.Phone.Number(),
                Type = (TelephoneType)Faker.RandomNumber.Next(0, 13),
                Preference = Faker.RandomNumber.Next(1, 10)
            };
        }

        public Email GenerateEmail()
        {
            return new Email
            {
                EmailAddress = Faker.Internet.Email(),
                Type = (EmailType)Faker.RandomNumber.Next(0, 11),
                Preference = Faker.RandomNumber.Next(1, 12)
            };
        }

        public Photo GetPhoto()
        {
            var path = Path.Combine(GetDataDirectory().FullName, PHOTO);
            return new Photo(true, ".jpeg", path);
        }

        public BusinessCard GenerateBusinessCard()
        {
            return new BusinessCard
            {
                FirstName = Faker.Name.First(),
                LastName = Faker.Name.Last(),
                Kind = (Kind)Faker.RandomNumber.Next(0, 3),
                Anniversary = Faker.RandomNumber.Next(0, 1) == 1 ? Faker.DateOfBirth.Next() : (DateTime?)null,
                Gender = (Gender)Faker.RandomNumber.Next(0, 4),
                Impps = GenerateModels(GenerateImpp, Faker.RandomNumber.Next(0, 10)),
                Languages = GenerateModels(GenerateLanguage, Faker.RandomNumber.Next(0, 10)),
                Relations = GenerateModels(GenerateRelation, Faker.RandomNumber.Next(0, 10)),
                Addresses = GenerateModels(GenerateAddress, Faker.RandomNumber.Next(0, 10)),
                DeliveryAddress = GenerateDeliveryAddress(),
                BirthDay = Faker.RandomNumber.Next(0, 1) == 1 ? Faker.DateOfBirth.Next() : (DateTime?)null,
                Emails = GenerateModels(GenerateEmail, Faker.RandomNumber.Next(0, 10)),
                Mailer = Faker.Internet.Email(),
                Photo = Faker.RandomNumber.Next(0, 1) == 1 ? GetPhoto() : default 
            };
        }

        public List<BusinessCard> GenerateBusinessCards() 
            => GenerateModels(GenerateBusinessCard, Faker.RandomNumber.Next(2, 10));

        private static List<T> GenerateModels<T>(Func<T> ctor, int count)
        {
            var models = new List<T>();
            for (int i = 0; i < count; i++)
            {
                models.Add(ctor());
            }
            return models;
        }
    }
}