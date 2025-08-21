namespace Foodiee.Co_WebApi.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public ICollection<Food> Foods { get; set; }

    }
}
