using Azure;
using Azure.Data.Tables;

namespace ABC_Retail.Models;

public class ProductEntity : ITableEntity
{
    public string PartitionKey { get; set; } = "PRODUCT";
    public string RowKey { get; set; } = Guid.NewGuid().ToString("N");
    public string ProductName { get; set; } = "";
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}