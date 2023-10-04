using System.Text.Json.Nodes;

namespace System.Text.Json
{
    public static class JsonExtensions
    {
        public static T DeserializeRequestBody<T>(this JsonObject body)
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString,
            };

            return JsonSerializer.Deserialize<T>(body.ToJsonString(), options);
        }

    }
}
