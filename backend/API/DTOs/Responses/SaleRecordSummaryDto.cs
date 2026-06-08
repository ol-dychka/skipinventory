using Domain;

namespace API.DTOs.Responses;

public class SaleRecordSummaryDto(List<SaleRecord> records)
{
    public List<SaleRecordSummaryLine> Days { get; set; } =
    [.. records.GroupBy(r => r.Date).Select(g => new SaleRecordSummaryLine([.. g]))];
}

// gets salerecords with the same date
public class SaleRecordSummaryLine(List<SaleRecord> records)
{
    public DateTime Date { get; set; } = records[0].Date;
    public int Total { get; set; } =
        records.Aggregate(0, (acc, current) => acc + current.UnitsSold - current.UnitsReturned);
}
