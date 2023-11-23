using System.Text.Json;

namespace Statify.Models
{
    public static class SessionExtensions
    {
        public static T? GetObjectFromJson<T>(this ISession session, string key)
        {
            var data = session.GetString(key);
            return data is null ? default : JsonSerializer.Deserialize<T>(data);
        }

        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }
    }
}
