using MixERP.Net.VCards;
using Reflektiv.Speechless.Core.Domain.Contracts.Models;
using System;

namespace Reflektiv.Speechless.Core.Domain.Concretes.Models
{
    public class BusinessCard : IHasId<Guid>, IBusinessCard, ITemporal, ITrashable
    {
        public string Label { get; set; }

        public VCard Details { get; set; }

        public Period? Effective { get; set; }

        public Guid Id { get; set;  }
        public bool IsTrashed { get; set; }

        public BusinessCard()
        {

        }
    }
}