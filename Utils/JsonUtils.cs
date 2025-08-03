using System.Text.Json;

namespace SmallUrl.Utils
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
