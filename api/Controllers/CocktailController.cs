using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cocktails.Class;

namespace Cocktails.Controllers
{
    [ApiController]
    [Route("api/cocktail")]
    public class CocktailController : ControllerBase
    {
        private readonly IMongoCollection<Cocktail> _collection;
        private readonly ILogger<Cocktail> _logger;

        public CocktailController(IMongoClient mongoClient, ILogger<Cocktail> logger)
        {
            var database = mongoClient.GetDatabase("cocktails_project");
            _collection = database.GetCollection<Cocktail>("cocktails");
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cocktail>>> Get()
        {
            try {
                var filter = Builders<Cocktail>.Filter.Empty;
                var cocktails = await _collection.Find(filter).ToListAsync();
                _logger.LogInformation($"Retrieved {cocktails.Count} cocktails");
                return Ok(cocktails);
            } catch (Exception ex)
            {
                _logger.LogError(ex, "error retrieving cocktails");
                return StatusCode(500, "An error occured while retrieving the data");
            }
        }

        [HttpGet("{idCocktail}")]
        public async Task<ActionResult> GetCocktail(string idCocktail)
        {
            if (!ObjectId.TryParse(idCocktail, out _)) {
                return BadRequest(new {message = "Invalid Id Format"});
            }

            var cocktail = await _collection.Find(u => u.IdCocktail == idCocktail).FirstOrDefaultAsync();
            if (cocktail == null) {
                return NotFound(new {message = "cocktail not found"});
            }
            return Ok(cocktail);
        }

        [HttpPost]
        public async Task<IActionResult> AddCocktail([FromBody] Cocktail cocktail)
        {
            if (cocktail == null) {
                return BadRequest("Cocktail data is null");
            }
            await _collection.InsertOneAsync(cocktail);

            return CreatedAtAction(nameof(AddCocktail), new {id = cocktail.IdCocktail}, cocktail);
        }

        [HttpPut("{idCocktail}")]
        public async Task<IActionResult> ModifyCocktail(string idCocktail, [FromBody] Cocktail updatedCocktail)
        {
            if (!ObjectId.TryParse(idCocktail, out _)) {
                return BadRequest(new {message = "Invalid Id Format"});
            }

            Cocktail? cocktail = await _collection.Find(u => u.IdCocktail == idCocktail).FirstOrDefaultAsync();
            if (cocktail == null) {
                return NotFound(new {message = "cocktail not found"});
            }
            if (!string.IsNullOrEmpty(updatedCocktail.Name)) cocktail.Name = updatedCocktail.Name;
            if (!string.IsNullOrEmpty(updatedCocktail.Picture)) cocktail.Name = updatedCocktail.Name;
            if (!string.IsNullOrEmpty(updatedCocktail.GlassType)) cocktail.GlassType = updatedCocktail.GlassType;
            if (!string.IsNullOrEmpty(updatedCocktail.Description)) cocktail.Description = updatedCocktail.Description;
            if (updatedCocktail.AlcoolFree == null) cocktail.AlcoolFree = updatedCocktail.AlcoolFree;
            if (updatedCocktail.Difficulty == null) cocktail.Difficulty = updatedCocktail.Difficulty;
            if (updatedCocktail.Popularity == null) cocktail.Popularity = updatedCocktail.Popularity;
            if (!string.IsNullOrEmpty(updatedCocktail.Recipe)) cocktail.Recipe = updatedCocktail.Recipe;
            if (updatedCocktail.PreparationTime == null) cocktail.PreparationTime = updatedCocktail.PreparationTime;
            
            await _collection.ReplaceOneAsync(u => u.IdCocktail == idCocktail, cocktail);
            return Ok(cocktail);
        }

        [HttpDelete("{idCocktail}")]
        public async Task<IActionResult> DeleteCocktail(string idCocktail)
        {
            if (!ObjectId.TryParse(idCocktail, out _)) {
                return BadRequest(new {message = "Invalid Id Format"});
            }

            Cocktail? cocktail = await _collection.Find(u => u.IdCocktail == idCocktail).FirstOrDefaultAsync();
            if (cocktail == null) {
                return NotFound(new {message = "cocktail not found"});
            }
            await _collection.DeleteOneAsync(u => u.IdCocktail == idCocktail);
            return Ok();
        }
    }
}