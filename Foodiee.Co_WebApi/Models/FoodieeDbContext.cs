using Foodiee.Co_WebApi.Models;
using Microsoft.EntityFrameworkCore;

public class FoodieeDbContext : DbContext
{
    public FoodieeDbContext(DbContextOptions<FoodieeDbContext> options) : base(options) { }

    public DbSet<Food> Foods { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<FoodIngredient> FoodIngredients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Many-to-Many Config
        modelBuilder.Entity<FoodIngredient>()
            .HasKey(fi => new { fi.FoodId, fi.IngredientId });

        modelBuilder.Entity<FoodIngredient>()
            .HasOne(fi => fi.Food)
            .WithMany(f => f.FoodIngredients)
            .HasForeignKey(fi => fi.FoodId);

        modelBuilder.Entity<FoodIngredient>()
            .HasOne(fi => fi.Ingredient)
            .WithMany(i => i.FoodIngredients)
            .HasForeignKey(fi => fi.IngredientId);
    }
}
