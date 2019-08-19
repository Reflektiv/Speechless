using Couchbase.Lite;
using Couchbase.Lite.Query;
using reexmonkey.xmisc.core.io.serializers;
using Reflektiv.Speechless.Core.Domain.Concretes.Models;
using Speechless.Infrastructure.Repositories.CouchbaseLite.Helpers;
using System;
using System.Linq.Expressions;

namespace Speechless.Infrastructure.Repositories.CouchbaseLite.Extensions
{
    public static class CouchbaseExtensions
    {
        public static IExpression AsExpression<TResult>(this Expression<Func<TResult, bool>> predicate)
            => Visitor.CreateFromExpression(predicate.Body).Visit();

        public static BusinessCard AsBusinessCard(this DictionaryObject map, TextSerializerBase serializer)
        {
            var json = map.GetString(nameof(BusinessCard));
            return !string.IsNullOrEmpty(json)
                ? json.AsModel<BusinessCard>(serializer)
                : default;
        }

        public static BusinessCard AsBusinessCard(this Document document, TextSerializerBase serializer)
        {
            var content = serializer.Serialize(document.GetString(nameof(BusinessCard)));
            return !string.IsNullOrEmpty(content)
                ? content.AsModel<BusinessCard>(serializer)
                : default;
        }

        public static T AsModel<T>(this string json, TextSerializerBase serializer)
        {
            var model = serializer.Deserialize<T>(json);
            return model != null ? model : default;
        }
    }
}
