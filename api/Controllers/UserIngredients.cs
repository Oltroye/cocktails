using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cocktails.Class;


namespace Cocktails.Controllers
{
    [ApiController]
    [Route("api/userIngredients")]
    public class UserIngredientsController : ControllerBase
    {
        private readonly IMongoCollection<UserIngredients> _collection;
        private readonly ILogger<UserIngredients> _logger;
        private readonly IMongoDatabase database;
        public UserIngredientsController(IMongoClient mongoClient, ILogger<UserIngredients> logger)
        {
            database = mongoClient.GetDatabase("cocktails_project");
            _collection = database.GetCollection<UserIngredients>("userIngredients");
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserIngredients>>> Get()
        {
            try {
                var filter = Builders<UserIngredients>.Filter.Empty;
                var userIngredients = await _collection.Find(filter).ToListAsync();
                _logger.LogInformation($"Retrieved {userIngredients.Count} userIngredientss");
                return Ok(userIngredients);
            } catch (Exception ex) {
                _logger.LogError(ex, "Error retrieving userIngredients");
                return StatusCode(500, "An error occured while retrieving the data");
            }
        }

        [HttpGet("{idUser}")]
        public async Task<ActionResult> GetUserIngredients(string idUser)
        {
            if (!ObjectId.TryParse(idUser, out _)) {
                return BadRequest(new {message = "Invalid ID format"});
            }
            var userIngredients = await _collection.Find(u => u.IdUser == idUser).ToListAsync();

            if (userIngredients == null) {
                return NotFound(new {message = "UserIngredients not found"});
            }
            return Ok(userIngredients);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserIngredients([FromBody] UserIngredients userIngredients)
        {
            if (userIngredients == null) {
                return BadRequest("UserIngredients data is null");
            }

            await _collection.InsertOneAsync(userIngredients);
            return CreatedAtAction(nameof(AddUserIngredients), new {id = userIngredients.IdUser }, userIngredients);
        }

        [HttpPut("{idUser,idIngredients}")]
        public async Task<IActionResult> ModifyUserIngredients(string idUser, string idIngredients, [FromBody] UserIngredients updatedUserIngredients)
        {
            if (!ObjectId.TryParse(idUser, out _) || !ObjectId.TryParse(idIngredients, out _)) {
                return BadRequest(new {message = "Invalid ID format"});
            }
            UserIngredients? userIngredients = await _collection.Find(u => u.IdUser == idUser && u.IdIngredients == idIngredients).FirstOrDefaultAsync();

            if (userIngredients == null) {
                return NotFound(new {message = "UserIngredients not found"});
            }
            
            if (!string.IsNullOrEmpty(updatedUserIngredients.IdIngredients)) userIngredients.IdIngredients = updatedUserIngredients.IdIngredients;
            if (updatedUserIngredients.isOwned == null) userIngredients.isOwned = updatedUserIngredients.isOwned;
            await _collection.ReplaceOneAsync(u => u.IdUser == idUser && u.IdIngredients == idIngredients, userIngredients);
            return Ok(userIngredients);
        }

        [HttpDelete("{idUser,idIngredients}")]
        public async Task<IActionResult> DeleteUserIngredients(string idUser, string idIngredients)
        {
            if (!ObjectId.TryParse(idUser, out _) || !ObjectId.TryParse(idIngredients, out _)) {
                return BadRequest(new {message = "Invalid ID format"});
            }
            UserIngredients? userIngredients = await _collection.Find(u => u.IdUser == idUser && u.IdIngredients == idIngredients).FirstOrDefaultAsync();

            if (userIngredients == null) {
                return NotFound(new {message = "UserIngredients not found"});
            }

            await _collection.DeleteOneAsync(u => u.IdUser == idUser && u.IdIngredients == idIngredients);
            return Ok();
        }
    }
}