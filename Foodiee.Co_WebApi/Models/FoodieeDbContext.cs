using Foodiee.Co_WebApi.Models;
using Microsoft.EntityFrameworkCore;

public class FoodieeDbContext : DbContext
{
    public FoodieeDbContext(DbContextOptions<FoodieeDbContext> options) : base(options) { }

    public DbSet<Food> Foods { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<FoodIngredient> FoodIngredients { get; set; }
    public DbSet<User> Users { get; set; }

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

        // One-to-Many Config (Category-Food)
        modelBuilder.Entity<Food>()
            .HasOne(f => f.Category)
            .WithMany(c => c.Foods)
            .HasForeignKey(f => f.CategoryId);

        // One-to-Many Config (User-Food)
        modelBuilder.Entity<Food>()
            .HasOne(f => f.User)
            .WithMany(u => u.Foods)
            .HasForeignKey(f => f.UserId);

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId);
            entity.HasIndex(e => e.Email).IsUnique();
        });

        modelBuilder.Entity<User>().HasData(
            new User {UserId=1,Name="Varsha",Email="vars@gmail.com",Password="123456" }
            );
    }
}
