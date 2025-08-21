using Foodiee.Co_WebApi.DTO;
using System.Text.Json.Serialization;

namespace Foodiee.Co_WebApi.Models
{
    public class RootObject
    {
        [JsonPropertyName("foods")]
        public List<FoodDto> Foods { get; set; }
        [JsonPropertyName("categories")]
        public List<CategoryDto> Categories { get; set; }

    }
}
