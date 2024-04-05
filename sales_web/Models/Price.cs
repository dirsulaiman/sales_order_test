namespace SalesOrderWeb.Models;

public class Price
{
    public int PriceId { get; set; }
    public string ProductCode { get; set; }
    public decimal PriceValue { get; set; }
    public DateTime PriceValidateFrom { get; set; }
    public DateTime PriceValidateTo { get; set; }
}