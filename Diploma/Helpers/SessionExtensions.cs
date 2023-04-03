using Newtonsoft.Json;

namespace Diploma.Helpers;

public static class SessionExtensions {
    public static void Set<T>(this ISession session, string key, T value)
    {
        session.SetString(key, JsonConvert.SerializeObject(value,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
    }

    public static T? Get<T>(this ISession session, string key) {
        var value = session.GetString(key);
        return value == null ? default : JsonConvert.DeserializeObject<T>(value,
            new JsonSerializerSettings {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
    }
}