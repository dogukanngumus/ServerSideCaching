using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace StackExchangeRedis.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RedisStringController:ControllerBase
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _cache;
    public RedisStringController(IConnectionMultiplexer redis)
    {
        _redis = redis;
        _cache = _redis.GetDatabase(0);
    }
    
    [HttpGet("[action]")]
    public IActionResult Set()
    {
        _cache.StringSet("name", "Dogukan");
        _cache.StringSet("lastName", "Gumus");
        _cache.StringSet("age", "23");
        return Ok();
    }
    
    [HttpGet("[action]")]
    public IActionResult Get()
    {
        string name = _cache.StringGet("name");
        if (String.IsNullOrEmpty(name))
        {
            return BadRequest();
        }
        return Ok(name);
    }
    
    
    [HttpGet("[action]")]
    public IActionResult Increment()
    {
        _cache.StringIncrement("age");
        string ageIncrement = _cache.StringGet("age");
        if (String.IsNullOrEmpty(ageIncrement))
        {
            return BadRequest();
        }
        return Ok(ageIncrement);
    }
    [HttpGet("[action]")]
    public IActionResult Decrement()
    {
        _cache.StringDecrement("age");
        string ageIncrement = _cache.StringGet("age");
        if (String.IsNullOrEmpty(ageIncrement))
        {
            return BadRequest();
        }
        return Ok(ageIncrement);
    }
}