using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sales_api.Models;
using sales_api.Models.DbContext;

namespace sales_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly AppDbContext _context;
    
    public ProductController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> Get(string? withPrices = "false", DateTime? orderDate = null)
    {
        orderDate = (orderDate ?? DateTime.Today).ToUniversalTime();
        if (withPrices == "true" || withPrices == "1")
        {
            var productsWithPrices = await _context.Product
                .Include(p => p.Prices)
                .Select(p => new Product()
                {
                    ProductCode = p.ProductCode,
                    ProductName = p.ProductName,
                    Prices = p.Prices
                        .OrderByDescending(price => price.PriceValidateFrom) // order by latest PriceValidateFrom
                        .Where(price => price.PriceValidateFrom <= orderDate && 
                                        price.PriceValidateTo >= orderDate)
                        .Select(price => new Price()
                        {
                            PriceId = price.PriceId,
                            PriceValue = price.PriceValue,
                            PriceValidateFrom = price.PriceValidateFrom,
                            PriceValidateTo = price.PriceValidateTo
                        }).ToList()
                })
                .ToListAsync();

            return productsWithPrices;
        }
        else
        {
            return await _context.Product.ToListAsync();
        }
    }
}
