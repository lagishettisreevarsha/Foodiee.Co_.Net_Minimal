using System.Text.Json;

namespace Foodiee.Co_WebApi.Models
{
    public static class DbSeeder
    {
        public static void Seed(FoodieeDbContext context)
        {
            if (!context.Categories.Any())
            {
                var jsonData = File.ReadAllText("foods.json");
                var root = JsonSerializer.Deserialize<RootObject>(jsonData);

                // Save categories
                var categories = root.Categories
                    .Select(c => new Category { Name = c.Name })
                    .ToList();

                context.Categories.AddRange(categories);
                context.SaveChanges();

                // Save foods
                foreach (var food in root.Foods)
                {
                    var category = categories.First(c => c.Name == food.Category);

                    var foodEntity = new Food
                    {
                        Title = food.Title,
                        Description = food.Description,
                        Image = food.Image,
                        Video = food.Video,
                        CategoryId = category.CategoryId
                       
                    };

                    context.Foods.Add(foodEntity);
                    context.SaveChanges();

                    // Add ingredients
                    foreach (var ingredientName in food.Ingredients)
                    {
                        var ingredient = context.Ingredients
                            .FirstOrDefault(i => i.Name == ingredientName)
                            ?? new Ingredient { Name = ingredientName };

                        if (ingredient.IngredientId == 0)
                            context.Ingredients.Add(ingredient);

                        context.SaveChanges();

                        context.FoodIngredients.Add(new FoodIngredient
                        {
                            FoodId = foodEntity.FoodId,
                            IngredientId = ingredient.IngredientId
                        });
                        context.SaveChanges();
                    }
                    context.SaveChanges();
                }
            }
        }
    }

}
