using MixERP.Net.VCards;
using Reflektiv.Speechless.Core.Domain.Concretes.Models;
using System;
using System.Collections.Generic;

namespace Reflektiv.Speechless.Core.Domain.Concretes.Extensions
{
    public static class BusinessCardExtensions
    {
        public static void PopulateFrom(this BusinessCard card, VCard vcard)
        {
            card.FirstName = vcard.FirstName;
            card.MiddleName = vcard.MiddleName;
            card.Prefix = vcard.Prefix;
            card.Suffix = vcard.Suffix;
            card.NickName = vcard.NickName;
            card.SortString = vcard.SortString;
            card.Sound = vcard.Sound;
            card.LastName = vcard.LastName;
            card.Key = vcard.Key;
            card.Source = vcard.Source;
            card.Kind = vcard.Kind;
            card.Anniversary = vcard.Anniversary;
            card.Gender = vcard.Gender;
            card.FormattedName = vcard.FormattedName;
            card.Longitude = vcard.Longitude;
            card.BirthDay = vcard.BirthDay;
            card.Mailer = vcard.Mailer;
            card.Title = vcard.Title;
            card.Role = vcard.Role;
            card.Latitude = vcard.Latitude;
            card.TimeZone = vcard.TimeZone;
            card.Photo = vcard.Photo;
            card.Note = vcard.Note;
            card.LastRevision = vcard.LastRevision;
            card.Url = vcard.Url;
            card.UniqueIdentifier = vcard.UniqueIdentifier;
            card.Version = vcard.Version;
            card.Organization = vcard.Organization;
            card.OrganizationalUnit = vcard.OrganizationalUnit;
            card.Classification = vcard.Classification;
            card.DeliveryAddress = vcard.DeliveryAddress;
            card.Logo = vcard.Logo;
            card.Categories = vcard.Categories;
            card.Impps = vcard.Impps;
            card.Languages = vcard.Languages;
            card.Relations = vcard.Relations;
            card.CalendarUserAddresses = vcard.CalendarUserAddresses;
            card.CalendarAddresses = vcard.CalendarAddresses;
            card.Addresses = vcard.Addresses;
            card.Telephones = vcard.Telephones;
            card.Emails = vcard.Emails;
            card.CustomExtensions = vcard.CustomExtensions;
        }

        public static BusinessCard ToBusinessCard(this VCard vcard)
        {
            return new BusinessCard
            {
                FirstName = vcard.FirstName,
                MiddleName = vcard.MiddleName,
                Prefix = vcard.Prefix,
                Suffix = vcard.Suffix,
                NickName = vcard.NickName,
                SortString = vcard.SortString,
                Sound = vcard.Sound,
                LastName = vcard.LastName,
                Key = vcard.Key,
                Source = vcard.Source,
                Kind = vcard.Kind,
                Anniversary = vcard.Anniversary,
                Gender = vcard.Gender,
                FormattedName = vcard.FormattedName,
                Longitude = vcard.Longitude,
                BirthDay = vcard.BirthDay,
                Mailer = vcard.Mailer,
                Title = vcard.Title,
                Role = vcard.Role,
                Latitude = vcard.Latitude,
                TimeZone = vcard.TimeZone,
                Photo = vcard.Photo,
                Note = vcard.Note,
                LastRevision = vcard.LastRevision,
                Url = vcard.Url,
                UniqueIdentifier = vcard.UniqueIdentifier,
                Version = vcard.Version,
                Organization = vcard.Organization,
                OrganizationalUnit = vcard.OrganizationalUnit,
                Classification = vcard.Classification,
                DeliveryAddress = vcard.DeliveryAddress,
                Logo = vcard.Logo,
                Categories = vcard.Categories,
                Impps = vcard.Impps,
                Languages = vcard.Languages,
                Relations = vcard.Relations,
                CalendarUserAddresses = vcard.CalendarUserAddresses,
                CalendarAddresses = vcard.CalendarAddresses,
                Addresses = vcard.Addresses,
                Telephones = vcard.Telephones,
                Emails = vcard.Emails,
                CustomExtensions = vcard.CustomExtensions,
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
