using cocktails.Models;
using cocktails.Lists;

using Microsoft.AspNetCore.Mvc;

namespace cocktails.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CocktailIngredientsController
    {
        Random rng = new Random();
        [HttpGet(Name = "GetCocktailIngredients")]
        public IEnumerable<CocktailIngredients> Get()
        {
            return new ListCocktailIngredients().List
                .OrderBy(_ => rng.Next())
                .Take(rng.Next(1, 6))
                .ToList();
        }
    }
}
