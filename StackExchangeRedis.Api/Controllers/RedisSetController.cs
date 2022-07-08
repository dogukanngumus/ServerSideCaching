using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace StackExchangeRedis.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RedisSetController:ControllerBase
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _cache;
    public RedisSetController(IConnectionMultiplexer redis)
    {
        _redis = redis;
        _cache = _redis.GetDatabase(0);
    }

    [HttpGet("[action]")]
    public IActionResult Set()
    {
        _cache.KeyExpire("color", DateTime.Now.AddMinutes(5));
        _cache.SetAdd("color", "blue");
        _cache.SetAdd("color", "red");
        return Ok();
    }
    
    [HttpGet("[action]")]
    public IActionResult Get()
    {
        HashSet<string> colors = new HashSet<string>();
        if (_cache.KeyExists("color"))
        {
            _cache.SetMembers("color").ToList().ForEach(x =>
            {
                colors.Add(x.ToString());
            });
        }

        return Ok(JsonSerializer.Serialize(colors));
    }

    
    [HttpGet("[action]")]
    public IActionResult Remove()
    {
        _cache.SetRemove("color", "blue");
        _cache.SetRemove("color", "red");
        return NoContent();
    }
}