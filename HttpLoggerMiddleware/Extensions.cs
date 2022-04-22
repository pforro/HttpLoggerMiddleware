#nullable disable
using Newtonsoft.Json;
using Microsoft.AspNetCore.Builder;

namespace HttpLoggerMiddleware
{
    /// <summary>
    /// 
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        public static IApplicationBuilder UseHttpLogger(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HttpLoggerMiddleware>();
        }

        /// <summary>
        /// Converts a KeyValuePair collection to a proper dictionary
        /// </summary>
        internal static IDictionary<TKey, TValue> ToDict<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> dict)
        {
            return dict?.ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// Converts an object to a dynamic typed Json object
        /// </summary>
        internal static dynamic ToJson(this object obj)
        {
            if (obj is string)
            {
                return JsonConvert
                    .DeserializeObject<dynamic>(obj as string);
            }

            var str = JsonConvert
                .SerializeObject(obj, Formatting.Indented);

            return JsonConvert
                .DeserializeObject<dynamic>(str);
        }
    }
}
