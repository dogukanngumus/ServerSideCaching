using InMemoryCache.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryCache.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HomeController : Controller
{
    private IMemoryCache _memoryCache;
    private readonly string _key;

    public HomeController(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
        _key = "myTestKey";
    }

    [HttpPost("[action]")]
    public IActionResult Set(string data)
    {
        _memoryCache.Set(_key, data);
        return Ok();
    }

    [HttpGet("[action]")]
    public IActionResult Get()
    {
        string value = _memoryCache.Get<string>(_key);
        return Ok(value);
    }
    
    [HttpGet("[action]")]
    public IActionResult Remove()
    {
        _memoryCache.Remove(_key);
        return Ok("Removed");
    }

    [HttpGet("[action]")]
    public IActionResult TryGetValue()
    {
        string value = "";
        bool isExists = _memoryCache.TryGetValue<string>(_key, out value);
        if (isExists)
            return Ok(value);
        return BadRequest();
    }
    
    [HttpGet("[action]")]
    public IActionResult GetOrCreate()
    {
        _memoryCache.GetOrCreate(_key, entry =>
        {
            return "GetOrCreate Successfully Worked !";
        });
        return Ok(_memoryCache.Get<string>(_key));
    }

    [HttpPost("[action]")]
    public IActionResult Expirations(string data)
    {
        MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
        options.SlidingExpiration = TimeSpan.FromSeconds(10);
        options.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
        options.Priority = CacheItemPriority.High;
        options.RegisterPostEvictionCallback((key,value,reason,state) =>
        {
            _memoryCache.Set("callback", $"key => {_key} value => {data} reason => {reason}");
        });
        _memoryCache.Set(_key, data, options);

        bool result = _memoryCache.TryGetValue(_key, out _);
        if (result)
            return Ok();
        return BadRequest();
    }

    [HttpPost("[action]")]
    public IActionResult CacheComplexTypes()
    {
        Product p = new Product()
        {
            Id = 1,
            Name = "Kalem",
            Price = 200
        };
        _memoryCache.Set<Product>(_key, p);
        return Ok(_memoryCache.Get<Product>(_key));
    }
    
}