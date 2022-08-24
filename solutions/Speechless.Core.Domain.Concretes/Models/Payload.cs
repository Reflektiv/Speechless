using Reflektiv.Speechless.Core.Domain.Contracts.Models;
using System;

namespace Reflektiv.Speechless.Core.Domain.Concretes.Models
{
    public abstract class Payload : IHasId<string>
    {
        public string Id { get; set; }

        public object Content { get; set; }
    }

    public abstract class Payload<T> : Payload
    {
        public new T Content { get => (T)base.Content; set => base.Content = value; }
    }

    public sealed class BusinessCardPayload : Payload<BusinessCard>
    {
    }
}
