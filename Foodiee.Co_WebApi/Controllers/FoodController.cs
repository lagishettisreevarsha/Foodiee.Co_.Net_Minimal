using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Foodiee.Co_WebApi.Models;
using Foodiee.Co_WebApi.DTO;

namespace Foodiee.Co_WebApi.Controllers
{
    //food controller
    [ApiController]
    [Route("api/Recipe")]
    public class FoodController : ControllerBase
    {
        private readonly FoodieeDbContext _context;

        public FoodController(FoodieeDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodDto>>> GetAllFoods()
        {
            try
            {
                var foods = await _context.Foods
                    .Include(f => f.Category)
                    .Include(f => f.FoodIngredients)
                        .ThenInclude(fi => fi.Ingredient)
                    .ToListAsync();

                var foodDtos = foods.Select(food => new FoodDto
                {
                    Id = food.FoodId,
                    Title = food.Title,
                    Description = food.Description,
                    Category = food.Category?.Name ?? "Unknown",
                    Image = food.Image,
                    Video = food.Video,
                    Ingredients = food.FoodIngredients?
                        .Select(fi => fi.Ingredient?.Name)
                        .Where(name => !string.IsNullOrEmpty(name))
                        .ToList() ?? new List<string>()
                }).ToList();

                return Ok(foodDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FoodDto>> GetFoodById(int id)
        {
            try
            {
                var food = await _context.Foods
                    .Include(f => f.Category)
                    .Include(f => f.FoodIngredients)
                        .ThenInclude(fi => fi.Ingredient)
                    .FirstOrDefaultAsync(f => f.FoodId == id);

                if (food == null)
                {
                    return NotFound($"Food with ID {id} not found");
                }

                var foodDto = new FoodDto
                {
                    Id = food.FoodId,
                    Title = food.Title,
                    Description = food.Description,
                    Category = food.Category?.Name ?? "Unknown",
                    Image = food.Image,
                    Video = food.Video,
                    Ingredients = food.FoodIngredients?
                        .Select(fi => fi.Ingredient?.Name)
                        .Where(name => !string.IsNullOrEmpty(name))
                        .ToList() ?? new List<string>()
                };

                return Ok(foodDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<FoodDto>>> GetFoodsByCategory(int categoryId)
        {
            try
            {
                var foods = await _context.Foods
                    .Include(f => f.Category)
                    .Include(f => f.FoodIngredients)
                        .ThenInclude(fi => fi.Ingredient)
                    .Where(f => f.CategoryId == categoryId)
                    .ToListAsync();

                var foodDtos = foods.Select(food => new FoodDto
                {
                    Id = food.FoodId,
                    Title = food.Title,
                    Description = food.Description,
                    Category = food.Category?.Name ?? "Unknown",
                    Image = food.Image,
                    Video = food.Video,
                    Ingredients = food.FoodIngredients?
                        .Select(fi => fi.Ingredient?.Name)
                        .Where(name => !string.IsNullOrEmpty(name))
                        .ToList() ?? new List<string>()
                }).ToList();

                return Ok(foodDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
