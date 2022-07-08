using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace StackExchangeRedis.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RedisListController:ControllerBase
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _cache;
    public RedisListController(IConnectionMultiplexer redis)
    {
        _redis = redis;
        _cache = _redis.GetDatabase(0);
    }
    
    [HttpGet("[action]")]
    public IActionResult Set()
    {
        _cache.ListLeftPush("kitaplar", "kitap1");
        _cache.ListRightPush("kitaplar", "kitap2");
        return Ok();
    }
    
    [HttpGet("[action]")]
    public IActionResult Get()
    {
        List<string> bookList = new List<string>();
        if (_cache.KeyExists("kitaplar"))
        {
            _cache.ListRange("kitaplar").ToList().ForEach(x =>
            {
                bookList.Add(x);
            });
        }
        return Ok(JsonSerializer.Serialize(bookList));
    }
    
    [HttpGet("[action]")]
    public IActionResult Remove()
    {
        if (_cache.KeyExists("kitaplar"))
        {
            _cache.ListRemove("kitaplar","kitap1");
            _cache.ListRemove("kitaplar","kitap2");
        }
        return NoContent();
    }
    
    [HttpGet("[action]")]
    public IActionResult Pop()
    {
        //_cache.ListRightPop("kitaplar");
        var popedData=  _cache.ListLeftPop("kitaplar");
        return Ok(popedData.ToString());
    }
}