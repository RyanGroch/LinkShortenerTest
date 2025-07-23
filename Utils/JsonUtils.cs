using System.Text.Json;

namespace LinkShortener.Utils
{
    public static class JsonUtils
    {
        public static T? Deserialize<T>(string? json)
        {
            try
            {
                return JsonSerializer.Deserialize<T>(json!);
            }
            catch
            {
                return default;
            }
        }
    }
}
