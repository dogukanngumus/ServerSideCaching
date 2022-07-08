using System.Text;
using System.Text.Json;
using DistributedCache.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace DistributedCache.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DistributedCacheController:ControllerBase
{
    private IDistributedCache _distributedCache;

    public DistributedCacheController(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<IActionResult> Index()
    {
        DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();

        cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(30);

        Product product = new Product { Id = 1, Name = "Kalem", Price = 100 };

        string jsonproduct = JsonSerializer.Serialize(product);

        Byte[] byteproduct = Encoding.UTF8.GetBytes(jsonproduct);

        _distributedCache.Set("product:1", byteproduct);

        //await _distributedCache.SetStringAsync("product:1", jsonproduct, cacheEntryOptions);

        return Ok();
    }

    public IActionResult Show()
    {
        Byte[] byteProduct = _distributedCache.Get("product:1");

        string jsonproduct = Encoding.UTF8.GetString(byteProduct);

        Product p = JsonSerializer.Deserialize<Product>(jsonproduct);
        return Ok(p);
    }

    public IActionResult Remove()
    {
        _distributedCache.Remove("name");

        return Ok();
    }

    public IActionResult ImageUrl()
    {
        byte[] resimbyte = _distributedCache.Get("resim");

        return File(resimbyte, "image/jpg");
    }

    public IActionResult ImageCache()
    {
        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/download.jpg");

        byte[] imageByte = System.IO.File.ReadAllBytes(path);

        _distributedCache.Set("resim", imageByte);

        return Ok();
    }
}