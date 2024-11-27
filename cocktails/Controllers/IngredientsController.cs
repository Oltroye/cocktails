﻿using Microsoft.AspNetCore.Mvc;

namespace WebApplication4.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IngredientsController
    {
        Random rng = new Random();
        [HttpGet(Name = "GetIngredients")]
        
        public IEnumerable<Ingredients> Get()
        {
            return new ListIngredients().List
                .OrderBy(_ => rng.Next())
                .Take(rng.Next(1, 6))
                .ToList();
        }
    }
}