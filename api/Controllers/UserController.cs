using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cocktails.Class;


namespace Cocktails.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IMongoCollection<User> _collection;
        private readonly ILogger<User> _logger;

        public UserController(IMongoClient mongoClient, ILogger<User> logger)
        {
            var database = mongoClient.GetDatabase("cocktails_project");
            _collection = database.GetCollection<User>("users");
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            try
            {
                var filter = Builders<User>.Filter.Empty;
                var users = await _collection.Find(filter).ToListAsync();
                _logger.LogInformation($"Retrieved {users.Count} users");
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users");
                return StatusCode(500, "An error occured while retrieving the data");
            }
        }

        [HttpGet("GetId/{idUser}")]
        public async Task<ActionResult> GetUserbyId(string idUser)
        {
            if (!ObjectId.TryParse(idUser, out _)) {
                return BadRequest(new { message = "Invalid ID format" });
            }
            var user = await _collection.Find(u => u.IdUser == idUser).FirstOrDefaultAsync();

            if (user == null) {
                return NotFound(new { message = "User not found" });
            }

            return Ok(user);
        }

        [HttpGet("Login/{email}")]
        public async Task<ActionResult> GetUserbyEmail(string email)
        {
            var user = await _collection.Find(u => u.Email == email).FirstOrDefaultAsync();
            if (user != null) return Ok(user); 
            else return NotFound(new { message = "User not found" });

        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            if (user == null) {
                return BadRequest("User data is null");
            }
            try
            {
                await _collection.InsertOneAsync(user);
            } catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                return Conflict("Email already used");
            }
            

            return CreatedAtAction(nameof(AddUser), new { id = user.IdUser });
        }

        [HttpPut("{idUser}")]
        public async Task<IActionResult> ModifyUser(string idUser, [FromBody] User updatedUser)
        {
            if (!ObjectId.TryParse(idUser, out _)) {
                return BadRequest("Invalid Id Format");
            }
            User? user = await _collection.Find(u => u.IdUser == idUser).FirstOrDefaultAsync();
            if (user == null) {
                return NotFound(new { message = "User not found" });
            }
            if (!string.IsNullOrEmpty(updatedUser.Name)) user.Name = updatedUser.Name;
            if (!string.IsNullOrEmpty(updatedUser.UserName)) user.UserName = updatedUser.UserName;
            if (!string.IsNullOrEmpty(updatedUser.Email)) user.Email = updatedUser.Email;
            if (!string.IsNullOrEmpty(updatedUser.Password)) user.Password = updatedUser.Password;
            if (!string.IsNullOrEmpty(updatedUser.FirstName)) user.FirstName = updatedUser.FirstName;

            await _collection.ReplaceOneAsync(u => u.IdUser == idUser, user);
            return Ok(user);
        }

        [HttpDelete("{idUser}")]
        public async Task<IActionResult> DeleteUser(string idUser)
        {
            if (!ObjectId.TryParse(idUser, out _)) {
                return BadRequest("Invalid Id Format");
            }
            User? user = await _collection.Find(u => u.IdUser == idUser).FirstOrDefaultAsync();
            if (user == null) {
                return NotFound(new { message = "User not found" });
            }

            await _collection.DeleteOneAsync(u => u.IdUser == idUser);
            return Ok();
        }

    }
}
