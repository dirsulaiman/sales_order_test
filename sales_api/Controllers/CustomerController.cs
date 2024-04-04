using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sales_api.Models;
using sales_api.Models.DbContext;

namespace sales_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly AppDbContext _context;
    
    public CustomerController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> Get()
    {
        return await _context.Customer.ToListAsync();
    }
}
