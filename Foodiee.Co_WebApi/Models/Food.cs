namespace Foodiee.Co_WebApi.Models
{
    public class Food
    {
        public int FoodId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string? Video { get; set; }

        // FK
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        // Many-to-Many
        public ICollection<FoodIngredient> FoodIngredients { get; set; }

    }
}
