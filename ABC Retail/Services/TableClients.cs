using Azure.Data.Tables;

namespace ABC_Retail.Services
{
    public class TableClients
    {
        public TableClient Customers { get; }
        public TableClient Products { get; }

        public TableClients(TableClient customers, TableClient products)
        {
            Customers = customers;
            Products = products;
        }
    }
}
