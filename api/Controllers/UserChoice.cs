using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cocktails.Class;


namespace Cocktails.Controllers
{
    [ApiController]
    [Route("api/userChoice")]
    public class UserChoiceController : ControllerBase
    {
        private readonly IMongoCollection<UserChoice> _collection;
        private readonly ILogger<UserChoice> _logger;
        private readonly IMongoDatabase database;
        
        public UserChoiceController(IMongoClient mongoClient, ILogger<UserChoice> logger)
        {
            database = mongoClient.GetDatabase("cocktails_project");
            _collection = database.GetCollection<UserChoice>("userChoice");
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserChoice>>> Get()
        {
            try {
                var filter = Builders<UserChoice>.Filter.Empty;
                var userChoice = await _collection.Find(filter).ToListAsync();
                _logger.LogInformation($"Retrieved {userChoice.Count} userChoices");
                return Ok(userChoice);
            } catch (Exception ex) {
                _logger.LogError(ex, "Error retrieving userChoice");
                return StatusCode(500, "An error occured while retrieving the data");
            }
        }

        [HttpGet("{idUser}")]
        public async Task<ActionResult> GetUserChoice(string idUser)
        {
            if (!ObjectId.TryParse(idUser, out _)) {
                return BadRequest(new {message = "Invalid ID format"});
            }
            var userChoice = await _collection.Find(u => u.IdUser == idUser).ToListAsync();

            if (userChoice == null) {
                return NotFound(new {message = "UserChoice not found"});
            }
            return Ok(userChoice);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserChoice([FromBody] UserChoice userChoice)
        {
            if (userChoice == null) {
                return BadRequest("UserChoice data is null");
            }
            var idUser = userChoice.IdUser;
            var idCocktail = userChoice.IdCocktail;
            IMongoCollection<UserIngredients> _collectionIngredientsUser = database.GetCollection<UserIngredients>("userIngredients");
            IMongoCollection<CocktailIngredients> _collectionIngredientsCocktail = database.GetCollection<CocktailIngredients>("CocktailIngredients");
            var user = await _collectionIngredientsUser.Find(u => u.IdUser == idUser).ToListAsync();
            var cocktails = await _collectionIngredientsCocktail.Find(u => u.IdCocktail == idCocktail).ToListAsync();
            foreach (var item in cocktails)
            {
                var ingredients = await _collectionIngredientsUser.Find(u => u.IdUser == idUser && u.IdIngredients == item.idIngredient).FirstOrDefaultAsync();
                if (ingredients != null)
                {
                    UserIngredients replace = new UserIngredients() 
                    {
                        IdIngredients = item.idIngredient,
                        IdUser = idUser,
                        isOwned = true, 
                    };
                    await _collectionIngredientsUser.ReplaceOneAsync(u => u.IdUser == idUser && u.IdIngredients == item.idIngredient, replace);
                } else {
                    UserIngredients replace = new UserIngredients()
                    {
                        IdIngredients = item.idIngredient,
                        IdUser = idUser,
                        isOwned = true,
                    };
                    await _collectionIngredientsUser.InsertOneAsync(replace);
                }
            }
            return CreatedAtAction(nameof(AddUserChoice), new {id = userChoice.IdUser }, userChoice);
        }

        [HttpPut("{idUser,idCocktail}")]
        public async Task<IActionResult> ModifyUserChoice(string idUser, string idCocktail, [FromBody] UserChoice updatedUserChoice)
        {
            if (!ObjectId.TryParse(idUser, out _) || !ObjectId.TryParse(idCocktail, out _)) {
                return BadRequest(new {message = "Invalid ID format"});
            }
            UserChoice? userChoice = await _collection.Find(u => u.IdUser == idUser && u.IdCocktail == idCocktail).FirstOrDefaultAsync();

            if (userChoice == null) {
                return NotFound(new {message = "UserChoice not found"});
            }
            
            if (!string.IsNullOrEmpty(updatedUserChoice.IdCocktail)) userChoice.IdCocktail = updatedUserChoice.IdCocktail;
            await _collection.ReplaceOneAsync(u => u.IdUser == idUser && u.IdCocktail == idCocktail, userChoice);
            return Ok(userChoice);
        }

        [HttpDelete("{idUser,idCocktail}")]
        public async Task<IActionResult> DeleteUserChoice(string idUser, string idCocktail)
        {
            if (!ObjectId.TryParse(idUser, out _) || !ObjectId.TryParse(idCocktail, out _)) {
                return BadRequest(new {message = "Invalid ID format"});
            }
            UserChoice? userChoice = await _collection.Find(u => u.IdUser == idUser && u.IdCocktail == idCocktail).FirstOrDefaultAsync();

            if (userChoice == null) {
                return NotFound(new {message = "UserChoice not found"});
            }

            await _collection.DeleteOneAsync(u => u.IdUser == idUser && u.IdCocktail == idCocktail);
            return Ok();
        }
    }
}