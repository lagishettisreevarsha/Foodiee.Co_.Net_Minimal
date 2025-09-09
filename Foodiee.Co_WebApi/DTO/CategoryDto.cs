using System.Text.Json.Serialization;

namespace Foodiee.Co_WebApi.DTO
{
    public class CategoryDto
        
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }

        // required
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

    }
}