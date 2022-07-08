using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace StackExchangeRedis.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RedisHashController:ControllerBase
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _cache;
    public RedisHashController(IConnectionMultiplexer redis)
    {
        _redis = redis;
        _cache = _redis.GetDatabase(0);
    }
    
    [HttpGet("[action]")]
    public IActionResult Get()
    {
        Dictionary<string, string> list = new Dictionary<string, string>();

        if (_cache.KeyExists("color"))
        {
            _cache.HashGetAll("color").ToList().ForEach(x =>
            {
                list.Add(x.Name, x.Value);
            });
        }

        return Ok(list);
    }

    [HttpGet("[action]")]
    public IActionResult Set()
    {
        _cache.HashSet("color", "name", "blue");

        return Ok();
    }
    
    [HttpGet("[action]")]
    public IActionResult Remove()
    {
        _cache.HashDelete("color", "name");
        return Ok();
    }
}