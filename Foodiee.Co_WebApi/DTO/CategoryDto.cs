
namespace Foodiee.Co_WebApi.DTO
{
    public class CategoryDto
    {
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }

    }
}
