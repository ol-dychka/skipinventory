using System;

namespace API.DTOs.Requests.SaleRecords;

public class CreateRequest
{
    public List<CreateRequestSaleRecord> Data { get; set; } = [];
    public DateOnly? Date { get; set; }
}

public class CreateRequestSaleRecord
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Sku { get; set; }
    public int Quantity { get; set; }
}
