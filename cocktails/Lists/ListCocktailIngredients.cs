using cocktails.Models;

namespace cocktails.Lists
{
    public class ListCocktailIngredients
    {
        public CocktailIngredients[] List => new[]
        {
            new CocktailIngredients
            {
                Quantity = 50,
                Units = "ml",
            },
            new CocktailIngredients
            {
                Quantity = 30,
                Units = "ml",
            },
            new CocktailIngredients
            {
                Quantity = 10,
                Units = "feuilles"
            },
            new CocktailIngredients
            {
                Quantity = 2,
                Units = "cuillières à café"
            },
            new CocktailIngredients
            {
                Quantity = 10,
                Units = "cl",
            }
        };
    }
}
