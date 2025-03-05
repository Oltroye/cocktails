namespace cocktails.Models
{
    public class Ingredients
    {
        public Guid UuidIngredients => Guid.NewGuid();
        //public int Quantity { get; set; }
        public Guid UuidType => Guid.NewGuid();
        public string Name { get; set; }
    }
}
