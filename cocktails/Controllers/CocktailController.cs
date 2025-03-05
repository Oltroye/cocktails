using cocktails.Models;
using cocktails.Lists;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;

namespace cocktails.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CocktailController
    {
        Random rng = new Random();
        [HttpGet(Name = "GetCocktail")]
        public IEnumerable<Cocktail> Get()
        {
            return new ListCocktails().List
                .OrderBy(_ => rng.Next())
                .Take(rng.Next(1,5))
                .ToList();
        }

    }
}