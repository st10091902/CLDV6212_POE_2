using Azure;
using Azure.Data.Tables;

namespace ABC_Retail.Services;

public class TableRepository<T> where T : class, ITableEntity, new()
{
    private readonly TableClient _table;

    public TableRepository(TableClient table) => _table = table;

    public async Task<List<T>> ListAsync(string? partitionKey = null)
    {
        var query = string.IsNullOrEmpty(partitionKey)
            ? _table.QueryAsync<T>()
            : _table.QueryAsync<T>(x => x.PartitionKey == partitionKey);

        var results = new List<T>();
        await foreach (var e in query) results.Add(e);
        return results;
    }

    public Task AddAsync(T entity) => _table.AddEntityAsync(entity);

    public Task DeleteAsync(string pk, string rk) =>
        _table.DeleteEntityAsync(pk, rk);
}
