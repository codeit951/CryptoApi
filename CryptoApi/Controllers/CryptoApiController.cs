using CryptoApi.Data;
using CryptoApi.Models;
using CryptoApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CryptoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CryptoApiController : ControllerBase
    {
        private readonly SqliteDataStorage _dataStorage;
        private readonly ILogger<CryptoApiController> _logger;

        public CryptoApiController(
            SqliteDataStorage dataStorage,
            ILogger<CryptoApiController> logger)
        {
            _dataStorage = dataStorage;
            _logger = logger;
        }

        // 1. Get all data
        [HttpGet]
        public IActionResult GetAll()
        {
            _logger.LogInformation("Fetching all cryptocurrency data");
            Dictionary<string, CryptoData> allData = _dataStorage.GetAllData()
                .ToDictionary(d => d.Symbol.ToUpper(), d => d);
            return Ok(allData);
        }

        // 2. Get first N items
        [HttpGet("count/{count}")]
        public IActionResult GetCount(int count)
        {
            _logger.LogInformation($"Fetching first {count} cryptocurrencies");

            if (count <= 0)
            {
                _logger.LogWarning($"Invalid count value: {count}");
                return BadRequest("Count must be a positive integer");
            }

            var data = _dataStorage.GetAllData()
                .OrderBy(d => d.Symbol)
                .Take(count)
                .ToList();

            Dictionary<string, CryptoData> dataDict = data
                .ToDictionary(d => d.Symbol.ToUpper(), d => d);

            return Ok(data);
        }

        // 2.1 Get first N items
        [HttpGet("random/count/{count}")]
        public IActionResult GetRandomCount(int count)
        {
            _logger.LogInformation($"Fetching first {count} cryptocurrencies");

            if (count <= 0)
            {
                _logger.LogWarning($"Invalid count value: {count}");
                return BadRequest("Count must be a positive integer");
            }

            var data = _dataStorage.GetAllData()
                .OrderBy(d => Guid.NewGuid())
                .Take(count)
                .ToList();

            Dictionary<string, CryptoData> dataDict = data
                .ToDictionary(d => d.Symbol.ToUpper(), d => d);

            return Ok(data);
        }

        // 3. Get by specific symbols
        [HttpGet("list")]
        public IActionResult GetBySymbols([FromQuery] string symbols)
        {
            if (string.IsNullOrWhiteSpace(symbols))
            {
                _logger.LogWarning("Symbols parameter missing in list request");
                return BadRequest("Symbols parameter is required");
            }

            var symbolList = symbols.Split(',')
                .Select(s => s.Trim().ToUpper())
                .ToList();

            _logger.LogInformation($"Fetching data for symbols: {symbols}");

            var data = _dataStorage.GetAllData()
                .Where(d => symbolList.Contains(d.Symbol.ToUpper()))
                .ToList();

            Dictionary<string, CryptoData> dataDict = data
                .ToDictionary(d => d.Symbol.ToUpper(), d => d);

            return Ok(data);
        }

        // 4. Search by query in symbols or names
        [HttpGet("search")]
        public IActionResult Search([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                _logger.LogWarning("Query parameter missing in search request");
                return BadRequest("Query parameter is required");
            }

            var cleanQuery = query.Trim().ToUpper();
            _logger.LogInformation($"Searching for: {cleanQuery}");

            var data = _dataStorage.GetAllData()
                .Where(d =>
                    d.Symbol.ToUpper().Contains(cleanQuery) ||
                    d.Name.ToUpper().Contains(cleanQuery))
                .ToList();

            Dictionary<string, CryptoData> dataDict = data
                .ToDictionary(d => d.Symbol.ToUpper(), d => d);

            return Ok(data);
        }
    }
}