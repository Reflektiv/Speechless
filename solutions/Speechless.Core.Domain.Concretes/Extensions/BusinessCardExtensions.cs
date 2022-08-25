using MixERP.Net.VCards;
using MixERP.Net.VCards.Models;
using Reflektiv.Speechless.Core.Domain.Concretes.Models;
using ServiceStack;
using System;
using System.Collections.Generic;
using Language = MixERP.Net.VCards.Models.Language;

namespace Reflektiv.Speechless.Core.Domain.Concretes.Extensions
{
    public static class BusinessCardExtensions
    {
        public static BusinessCard ToBusinessCard(this VCard card)
        {
            return new BusinessCard
            {
                FirstName = card.FirstName,
                MiddleName = card.MiddleName,
                Prefix = card.Prefix,
                Suffix = card.Suffix,
                NickName = card.NickName,
                SortString = card.SortString,
                Sound = card.Sound,
                LastName = card.LastName,
                Key = card.Key,
                Source = card.Source,
                Kind = card.Kind,
                Anniversary = card.Anniversary,
                Gender = card.Gender,
                FormattedName = card.FormattedName,
                Longitude = card.Longitude,
                BirthDay = card.BirthDay,
                Mailer = card.Mailer,
                Title = card.Title,
                Role = card.Role,
                Latitude = card.Latitude,
                TimeZone = card.TimeZone,
                Photo = card.Photo,
                Note = card.Note,
                LastRevision = card.LastRevision,
                Url = card.Url,
                UniqueIdentifier = card.UniqueIdentifier,
                Version = card.Version,
                Organization = card.Organization,
                OrganizationalUnit = card.OrganizationalUnit,
                Classification = card.Classification,
                DeliveryAddress = card.DeliveryAddress,
                Logo = card.Logo,
                Categories = card.Categories,
                Impps = card.Impps,
                Languages = card.Languages,
                Relations = card.Relations,
                CalendarUserAddresses = card.CalendarUserAddresses,
                CalendarAddresses = card.CalendarAddresses,
                Addresses = card.Addresses,
                Telephones = card.Telephones,
                Emails = card.Emails,
                CustomExtensions = card.CustomExtensions,
            };
        }

        public static VCard ToVCard(this BusinessCard card)
        {
            return new VCard
            {
                FirstName = card.FirstName,
                MiddleName = card.MiddleName,
                Prefix = card.Prefix,
                Suffix = card.Suffix,
                NickName = card.NickName,
                SortString = card.SortString,
                Sound = card.Sound,
                LastName = card.LastName,
                Key = card.Key,
                Source = card.Source,
                Kind = card.Kind,
                Anniversary = card.Anniversary,
                Gender = card.Gender,
                FormattedName = card.FormattedName,
                Longitude = card.Longitude,
                BirthDay = card.BirthDay,
                Mailer = card.Mailer,
                Title = card.Title,
                Role = card.Role,
                Latitude = card.Latitude,
                TimeZone = card.TimeZone,
                Photo = card.Photo,
                Note = card.Note,
                LastRevision = card.LastRevision,
                Url = card.Url,
                UniqueIdentifier = card.UniqueIdentifier,
                Version = card.Version,
                Organization = card.Organization,
                OrganizationalUnit = card.OrganizationalUnit,
                Classification = card.Classification,
                DeliveryAddress = card.DeliveryAddress,
                Logo = card.Logo,
                Categories = card.Categories,
                Impps = card.Impps,
                Languages = card.Languages,
                Relations = card.Relations,
                CalendarUserAddresses = card.CalendarUserAddresses,
                CalendarAddresses = card.Relations != null ? new List<Uri>(card.CalendarAddresses) : new List<Uri>(),
                Addresses = card.Addresses,
                Telephones = card.Telephones,
                Emails = card.Emails,
                CustomExtensions = card.CustomExtensions,
            };
        }
    }
}
