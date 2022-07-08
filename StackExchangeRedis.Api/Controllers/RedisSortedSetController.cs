using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace StackExchangeRedis.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RedisSortedSetController:ControllerBase
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _cache;
    public RedisSortedSetController(IConnectionMultiplexer redis)
    {
        _redis = redis;
        _cache = _redis.GetDatabase(0);
    }

    [HttpGet("[action]")]
    public IActionResult Set()
    {
        _cache.KeyExpire("kitaplar", DateTime.Now.AddMinutes(1));
        _cache.SortedSetAdd("kitaplar", "kitap 1", 1);
        _cache.SortedSetAdd("kitaplar", "kitap 2", 2);
        _cache.SortedSetAdd("kitaplar", "kitap 3", 4);
        _cache.SortedSetAdd("kitaplar", "kitap 4", 3);
        return NoContent();
    }
    
    [HttpGet("[action]")]
    public IActionResult Get()
    {
        HashSet<string> books = new HashSet<string>();
        if (_cache.KeyExists("kitaplar"))
        {
            _cache.SortedSetScan("kitaplar").ToList().ForEach(x =>
            {
                books.Add(x.ToString());
            });
        }

        return Ok(JsonSerializer.Serialize(books));
    }
    
    [HttpGet("[action]")]
    public IActionResult Remove()
    {
        _cache.SortedSetRemove("kitaplar", "kitap 1" );
        _cache.SortedSetRemove("kitaplar", "kitap 2" );
        _cache.SortedSetRemove("kitaplar", "kitap 3" );
        _cache.SortedSetRemove("kitaplar", "kitap 4" );
        return Ok();
    }
    
}