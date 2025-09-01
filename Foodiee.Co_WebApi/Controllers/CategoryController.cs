using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Foodiee.Co_WebApi.Models;
using Foodiee.Co_WebApi.DTO;

namespace Foodiee.Co_WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly FoodieeDbContext _context;

        public CategoryController(FoodieeDbContext context)
        {
            _context = context;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategories()
        {
            var categories = await _context.Categories
                .Select(c => new CategoryDto
                {
                    Id = c.CategoryId,
                    Name = c.Name
                })
                .ToListAsync();

            return Ok(categories);
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategoryById(int id)
        {
            var category = await _context.Categories
                .Where(c => c.CategoryId == id)
                .Select(c => new CategoryDto
                {
                    Id = c.CategoryId,
                    Name = c.Name
                })
                .FirstOrDefaultAsync();

            if (category == null)
            {
                return NotFound($"Category with ID {id} not found");
            }

            return Ok(category);
        }

        // GET: api/Category/5/foods
        [HttpGet("{id}/foods")]
        public async Task<ActionResult<IEnumerable<FoodDto>>> GetFoodsByCategory(int id)
        {
            var categoryExists = await _context.Categories.AnyAsync(c => c.CategoryId == id);
            if (!categoryExists)
            {
                return NotFound($"Category with ID {id} not found");
            }

            var foods = await _context.Foods
                .Include(f => f.Category)
                .Include(f => f.FoodIngredients)
                    .ThenInclude(fi => fi.Ingredient)
                .Where(f => f.CategoryId == id)
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
                    .Where(name => !string.IsNullOrWhiteSpace(name))
                    .ToList() ?? new List<string>()
            }).ToList();

            return Ok(foodDtos);
        }

        // POST: api/Category
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CategoryDto categoryDto)
        {
            if (categoryDto == null || string.IsNullOrWhiteSpace(categoryDto.Name))
            {
                return BadRequest("Category name is required");
            }

            var category = new Category
            {
                Name = categoryDto.Name.Trim()
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var created = new CategoryDto
            {
                Id = category.CategoryId,
                Name = category.Name
            };

            return CreatedAtAction(nameof(GetCategoryById), new { id = created.Id }, created);
        }

        // PUT: api/Category/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDto categoryDto)
        {
            if (categoryDto == null || id != categoryDto.Id)
            {
                return BadRequest("Invalid category payload");
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound($"Category with ID {id} not found");
            }

            if (string.IsNullOrWhiteSpace(categoryDto.Name))
            {
                return BadRequest("Category name is required");
            }

            category.Name = categoryDto.Name.Trim();

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound($"Category with ID {id} not found");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}