using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sales_api.Models;
using sales_api.Models.DbContext;

namespace sales_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesOrderController : ControllerBase
{
    private readonly AppDbContext _context;
    
    public SalesOrderController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SalesOrder>>> Get()
    {
        return await _context.SalesOrder.ToListAsync();
    }
}
