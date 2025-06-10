using System.Text.Json.Serialization;
using System.Text.Json;

namespace TrekkingApi.Domain.Options.JsonSerializeOptions
{
    public class JsonSerializeOptionsSet
    {
        public static readonly JsonSerializerOptions mainOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }
}
