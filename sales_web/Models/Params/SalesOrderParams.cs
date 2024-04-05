namespace SalesOrderWeb.Models.Params;

public class SalesOrderParams
{  
    public string CustCode { get; set; }
    public string ProductCode { get; set; }
    public int Qty { get; set; }
    public decimal Price { get; set; }
}