using Azure;
using Azure.Data.Tables;

namespace ABC_Retail.Models;

public class CustomerEntity : ITableEntity
{
    public string PartitionKey { get; set; } = "CUSTOMER";
    public string RowKey { get; set; } = Guid.NewGuid().ToString("N");
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Address { get; set; } = "";
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}