using Reflektiv.Speechless.Core.Domain.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Reflektiv.Speechless.Core.Domain.Concretes.Models
{
    [KnownType(typeof(BusinessCardPayload))]
    public class Peer : IHasId<Guid>
    {
        public Guid Id { get; set; }

        public string PublicKeyXml { get; set; }

        public List<Payload> Payloads { get; set; }

        public Peer()
        {
            Payloads = new List<Payload>();
        }
    }
}
