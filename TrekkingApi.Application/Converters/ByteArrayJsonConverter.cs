using System.Text.Json.Serialization;
using System.Text.Json;

namespace TrekkingApi.Application.Converters
{
    public class ByteArrayJsonConverter : JsonConverter<byte[]>
    {
        public override byte[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string base64UrlSafeString = reader.GetString();
                string base64StandardString = ConvertBase64UrlToBase64Standard(base64UrlSafeString);
                return Convert.FromBase64String(base64StandardString);
            }

            throw new JsonException("Невозможно десериализовать массив байтов.");
        }

        public override void Write(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options)
        {
            string base64UrlSafeString = ConvertBase64StandardToBase64Url(Convert.ToBase64String(value));
            writer.WriteStringValue(base64UrlSafeString);
        }

        private string ConvertBase64UrlToBase64Standard(string base64Url)
        {
            // Заменяем URL-safe символы на стандартные Base64
            string base64Standard = base64Url.Replace('-', '+').Replace('_', '/');

            // Добавляем заполнение '=' для кратности длины строки 4
            switch (base64Standard.Length % 4)
            {
                case 2: base64Standard += "=="; break;
                case 3: base64Standard += "="; break;
            }

            return base64Standard;
        }

        private string ConvertBase64StandardToBase64Url(string base64Standard)
        {
            // Заменяем стандартные Base64 символы на URL-safe
            return base64Standard.Replace('+', '-').Replace('/', '_').TrimEnd('=');
        }
    }
}
