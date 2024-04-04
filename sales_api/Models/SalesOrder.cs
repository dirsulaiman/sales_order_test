namespace sales_api.Models;

public class SalesOrder
{
    public string SalesOrderNo { get; set; }
    public DateTime OrderDate { get; set; }    
    public string CustCode { get; set; }
    public string ProductCode { get; set; }
    public int Qty { get; set; }
    public decimal Price { get; set; }
}