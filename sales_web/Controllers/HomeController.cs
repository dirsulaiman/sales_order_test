using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SalesOrderWeb.Models;
using SalesOrderWeb.Models.Params;
using SalesOrderWeb.Services;

namespace SalesOrderWeb.Controllers;

public class HomeController : Controller
{
    private RequestService _requestService;

    public HomeController(IConfiguration configuration)
    {
        _requestService = new RequestService(configuration);
    }

    public async Task<IActionResult> Index()
    {
        List<Customer> customers = await _requestService.Get<List<Customer>>("api/customer");
        List<Product> products = await _requestService.Get<List<Product>>("api/product");
        ViewBag.customers = customers;
        ViewBag.products = products;
        return View();
    }
    
    [HttpPost]
    public async Task<RedirectToActionResult> SubmitForm(SalesOrderParams formData)
    {
        try
        {
            await _requestService.Post("api/salesorder", new
            {
                custCode = formData.CustCode,
                productCode = formData.ProductCode,
                qty = formData.Qty,
                price = formData.Price
            });
            
            // Set success notification message
            TempData["NotificationMessage"] = "Data has been insert successfully.";
            
            // Redirect to another action method
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            // Set error notification message
            TempData["NotificationMessage"] = "An error occurred: " + ex.Message;
            
            // Redirect to another action method
            return RedirectToAction("Index", "Home");
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
