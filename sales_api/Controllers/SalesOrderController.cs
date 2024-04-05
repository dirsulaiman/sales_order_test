using System.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using sales_api.Models;
using sales_api.Models.DbContext;

namespace sales_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesOrderController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    
    public SalesOrderController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SalesOrder>>> Get()
    {
        return await _context.SalesOrder.ToListAsync();
    }
    
    [HttpPost]
    public async Task<IActionResult> InsertSalesOrder(SalesOrder salesOrder)
    {
        try
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            // Create a connection to the database
            using (var connection = new NpgsqlConnection(connectionString))
            {
                // Open the connection
                await connection.OpenAsync();

                // Begin a transaction
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        DateTime currentDate = DateTime.Today;
                        
                        // Execute the stored procedure within the transaction
                        await connection.ExecuteAsync("insertsalesorder", 
                            new 
                            {
                                p_order_date = currentDate,
                                p_cust_code = salesOrder.CustCode,
                                p_product_code = salesOrder.ProductCode,
                                p_qty = salesOrder.Qty,
                                p_price = salesOrder.Price
                            },
                            commandType: CommandType.StoredProcedure,
                            transaction: transaction);

                        // Commit the transaction if everything is successful
                        transaction.Commit();

                        return Ok(); // Or return some other response indicating success
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction if an exception occurs
                        transaction.Rollback();

                        // Handle exceptions
                        return StatusCode(500, ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions
            return StatusCode(500, ex.Message);
        }
    }
}
