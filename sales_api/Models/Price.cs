using System.ComponentModel.DataAnnotations.Schema;

namespace sales_api.Models;

public class Price
{
    public int PriceId { get; set; }
    public string ProductCode { get; set; }
    
    [Column("Price")]
    public decimal PriceValue { get; set; }
    public DateTime PriceValidateFrom { get; set; }
    public DateTime PriceValidateTo { get; set; }
    public Product Product { get; set; }
}