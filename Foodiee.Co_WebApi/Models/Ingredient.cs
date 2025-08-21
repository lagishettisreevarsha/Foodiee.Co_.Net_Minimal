namespace Foodiee.Co_WebApi.Models
{
    public class Ingredient
    {
        public int IngredientId { get; set; }
        public string Name { get; set; }

        public ICollection<FoodIngredient> FoodIngredients { get; set; }
    }
}
