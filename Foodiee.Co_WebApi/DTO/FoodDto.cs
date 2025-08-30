namespace Foodiee.Co_WebApi.DTO
{
    public class FoodDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }
        public string? Video { get; set; }
        public List<string> Ingredients { get; set; }

    }
}
