using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cocktails.Class;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;

namespace Cocktails.Controllers
{
    [ApiController]
    [Route("api/ingredients")]

    public class IngredientsController : ControllerBase
    {
        private readonly IMongoCollection<Ingredients> _collection;
        private readonly ILogger<Ingredients> _logger;

        public IngredientsController(IMongoClient mongoClient, ILogger<Ingredients> logger)
        {
            var database = mongoClient.GetDatabase("cocktails_project");
            _collection = database.GetCollection<Ingredients>("ingredients");
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ingredients>>> Get()
        {
            try {
                var filter = Builders<Ingredients>.Filter.Empty;
                var ingredients = await _collection.Find(filter).ToListAsync();
                _logger.LogInformation($"Retrieved {ingredients.Count} ingredients");
                return Ok(ingredients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ingredients");
                return StatusCode(500, "An error occured while retrieving the data");
            }
        }

        [HttpGet("{idIngredient}")]
        public async Task<ActionResult> GetIngredients(string idIngredient)
        {
            if (!ObjectId.TryParse(idIngredient, out _)) {
                return BadRequest(new { message = "Invalid ID format" });
            }
            var ingredient = await _collection.Find(u => u.IdIngredients == idIngredient).FirstOrDefaultAsync();
            if (ingredient == null) {
                return NotFound(new {message = "Ingredient not found" });
            }
            return Ok(ingredient);
        }

        [HttpPost]
        public async Task<IActionResult> AddIngredient([FromBody] Ingredients ingredient)
        {
            if (ingredient == null) {
                return BadRequest("ingredient data is null");
            }
            await _collection.InsertOneAsync(ingredient);
            return CreatedAtAction(nameof(AddIngredient), new {id = ingredient.IdIngredients}, ingredient);
        }

        [HttpDelete("{idIngredient}")]
        public async Task<IActionResult> DeleteIngredient(string idIngredient)
        {
            if (!ObjectId.TryParse(idIngredient, out _)) {
                return BadRequest(new { message = "Invalid ID format" });
            }
            var ingredient = await _collection.Find(u => u.IdIngredients == idIngredient).FirstOrDefaultAsync();
            if (ingredient == null) {
                return NotFound(new {message = "Ingredient not found" });
            }
            await _collection.DeleteOneAsync(u => u.IdIngredients == idIngredient);
            return Ok();
        }
    }
}