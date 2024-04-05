namespace SalesOrderWeb.Models;

public class Product
{
    public string ProductCode { get; set; }
    public string ProductName { get; set; }
    public List<Price>? Prices { get; set; }
}