namespace cocktails.Models
{
    public class CocktailIngredients
    {
        public Guid UuidCocktail => Guid.NewGuid();
        public Guid UuidIngredients => Guid.NewGuid();
        public int Quantity {  get; set; }
        public string Units { get; set; }

    }
}
