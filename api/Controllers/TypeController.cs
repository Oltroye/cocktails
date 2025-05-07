using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cocktails.Class;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage;


namespace Cocktails.Controllers
{
    [ApiController]
    [Route("api/type")]
    public class TypeController : ControllerBase
    {
        private readonly IMongoCollection<Class.Type> _collection;
        private readonly ILogger<Class.Type> _logger;

        public TypeController(IMongoClient mongoClient, ILogger<Class.Type> logger)
        {
            var database = mongoClient.GetDatabase("cocktails_project");
            _collection = database.GetCollection<Class.Type>("type");
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Class.Type>>> Get()
        {
            try
            {
                var filter = Builders<Class.Type>.Filter.Empty;
                var types = await _collection.Find(filter).ToListAsync();
                _logger.LogInformation($"Retrieved {types.Count} types");
                return Ok(types);
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving types");
                return StatusCode(500, "An error occured while retrieving the data");
            }
        }

        [HttpGet("{idType}")]
        public async Task<ActionResult> GetType(string idType)
        {
            if (!ObjectId.TryParse(idType, out _)) {
                return BadRequest(new {message = "invalid Id format"});
            }
            var type = await _collection.Find(u => u.IdType == idType).FirstOrDefaultAsync();

            if (type == null) {
                return NotFound(new {message = "Type not Found"});
            }
            return Ok(type);
        }

        [HttpPost]
        public async Task<IActionResult> AddType([FromBody] Class.Type type)
        {
            if (type == null) {
                return BadRequest("Type data is null");
            }
            await _collection.InsertOneAsync(type);
            return CreatedAtAction(nameof(AddType), new {id = type.IdType}, type);
        }

        [HttpDelete("{idType}")]
        public async Task<IActionResult> DeleteType(string idType)
        {
            if (!ObjectId.TryParse(idType, out _)) {
                return BadRequest(new {message = "invalid Id format"});
            }
            var type = await _collection.Find(u => u.IdType == idType).FirstOrDefaultAsync();

            if (type == null) {
                return NotFound(new {message = "Type not Found"});
            }

            await _collection.DeleteOneAsync(u => u.IdType == idType);
            return Ok();
        }
    }
}