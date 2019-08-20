using Couchbase.Lite;
using Couchbase.Lite.Query;
using reexmonkey.xmisc.core.io.serializers;
using Speechless.Infrastructure.Repositories.CouchbaseLite.Helpers;
using System;
using System.Linq.Expressions;

namespace Speechless.Infrastructure.Repositories.CouchbaseLite.Extensions
{
    public static class CouchbaseExtensions
    {
        public static IExpression AsExpression<TResult>(this Expression<Func<TResult, bool>> predicate)
            => VisitorBase.CreateFromExpression(predicate).Visit();

        public static T AsModel<T>(this Document document, TextSerializerBase serializer)
        {
            var json = serializer.Serialize(document);
            return !string.IsNullOrEmpty(json)
                ? json.AsModel<T>(serializer)
                : default;
        }

        public static T AsModel<T>(this DictionaryObject map, TextSerializerBase serializer)
        {
            var json = serializer.Serialize(map.ToDictionary());
            return !string.IsNullOrEmpty(json)
                ? json.AsModel<T>(serializer)
                : default;
        }

        public static T AsModel<T>(this string json, TextSerializerBase serializer)
        {
            var model = serializer.Deserialize<T>(json);
            return model != null ? model : default;
        }
    }
}