using System.Text.Json.Serialization;

namespace Foodiee.Co_WebApi.DTO
{
    public class FoodDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("category")]
        public string Category { get; set; }
        [JsonPropertyName("image")]
        public string Image { get; set; }
        [JsonPropertyName("video")]
        public string? Video { get; set; }
        [JsonPropertyName("ingredients")]
        public List<string> Ingredients { get; set; }

    }
}
