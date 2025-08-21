using System.Text.Json.Serialization;

namespace Foodiee.Co_WebApi.DTO
{
    public class CategoryDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }

    }
}
