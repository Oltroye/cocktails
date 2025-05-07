using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cocktails.Class;

namespace Cocktails.Controllers
{
    [ApiController]
    [Route("api/cocktailIngredient")]
    public class CocktailIngredientsController : ControllerBase
    {
        private readonly IMongoCollection<CocktailIngredients> _collection;
        private readonly ILogger<CocktailIngredients> _logger;

        public CocktailIngredientsController(IMongoClient mongoClient, ILogger<CocktailIngredients> logger)
        {
            var database = mongoClient.GetDatabase("cocktails_project");
            _collection = database.GetCollection<CocktailIngredients>("CocktailIngredients");
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CocktailIngredients>>> Get()
        {
            try {
                var filter = Builders<CocktailIngredients>.Filter.Empty;
                var cocktailIngredient = await _collection.Find(filter).ToListAsync();
                _logger.LogInformation($"Retrieved {cocktailIngredient.Count} cocktailIngredient");
                return Ok(cocktailIngredient);
            } catch (Exception ex) {
                _logger.LogError(ex, "Error retrieving cocktailsIngredients");
                return StatusCode(500, "An error occured while retrieving the data");
            }
        }

        [HttpGet("{idCocktail}")]
        public async Task<ActionResult> GetCocktailIngredient(string idCocktail)
        {
            if (!ObjectId.TryParse(idCocktail, out _)) {
                return BadRequest(new {message = "Invalid id Format"});
            }

            var cocktailIngredient = await _collection.Find(u => u.IdCocktail == idCocktail).ToListAsync();
            if (cocktailIngredient == null) {
                return NotFound(new {message = "CocktailIngredient not found"});
            }

            return Ok(cocktailIngredient);
        }

        [HttpPost]
        public async Task<IActionResult> AddCocktailIngredient([FromBody] CocktailIngredients cocktailIngredients)
        {
            if (cocktailIngredients == null) {
                return BadRequest("cocktailIngredients is null");
            }
            await _collection.InsertOneAsync(cocktailIngredients);

            return CreatedAtAction(nameof(AddCocktailIngredient),new {id = cocktailIngredients.IdCocktail}, cocktailIngredients);
        }

        [HttpPut("{idCocktail,idIngredient}")]
        public async Task<IActionResult> ModifyCocktailIngredients(string idCocktail, string idIngredient, [FromBody] CocktailIngredients updatedCocktailIngredients)
        {
            if (!ObjectId.TryParse(idCocktail, out _) || !ObjectId.TryParse(idIngredient, out _)) {
                return BadRequest("Invalid Id Format");
            }
            CocktailIngredients? cocktailIngredients = await _collection.Find(u => u.IdCocktail == idCocktail && u.idIngredient == idIngredient).FirstOrDefaultAsync();
            if (cocktailIngredients == null) {
                return NotFound(new { message = "CocktailIngredients not found" });
            }
            if (updatedCocktailIngredients.Quantity == null) cocktailIngredients.Quantity = updatedCocktailIngredients.Quantity;
            if (!string.IsNullOrEmpty(updatedCocktailIngredients.Units))

            await _collection.ReplaceOneAsync(u => u.IdCocktail == idCocktail, cocktailIngredients);
            return Ok(cocktailIngredients);
        }


        [HttpDelete("{idCocktail}")]
        public async Task<IActionResult> DeleteCocktailIngredients(string idCocktail)
        {
            if (!ObjectId.TryParse(idCocktail, out _)) {
                return BadRequest("Invalid Id Format");
            }
            CocktailIngredients? cocktailIngredients = await _collection.Find(u => u.IdCocktail == idCocktail).FirstOrDefaultAsync();
            if (cocktailIngredients == null) {
                return NotFound(new { message = "CocktailIngredients not found" });
            }

            await _collection.DeleteOneAsync(u => u.IdCocktail == idCocktail);
            return Ok();
        }
    }
}