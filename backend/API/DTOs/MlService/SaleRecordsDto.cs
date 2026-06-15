using Domain;

namespace API.DTOs.MlService;

public class SaleRecordsDto(List<SaleRecord> sales)
{
    public List<SaleRecord> Sales { get; set; } = sales;
    public ProductDto Product { get; set; } = new ProductDto(sales[0].Product);
}
