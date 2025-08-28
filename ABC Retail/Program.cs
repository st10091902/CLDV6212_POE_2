using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Azure.Storage.Files.Shares;
using ABC_Retail.Services;
using ABC_Retail.Models;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// Bind storage options
var cfg = builder.Configuration.GetSection("Storage");
string cs = cfg["ConnectionString"]!;
string blobContainer = cfg["BlobContainer"]!;
string queueName = cfg["QueueName"]!;
string fileShareName = cfg["FileShareName"]!;
string tableCustomers = cfg["TableCustomers"]!;
string tableProducts = cfg["TableProducts"]!;

// Azure clients (singletons)
builder.Services.AddSingleton(new TableServiceClient(cs));
builder.Services.AddSingleton(new BlobServiceClient(cs));
builder.Services.AddSingleton(new QueueServiceClient(cs));
builder.Services.AddSingleton(new ShareServiceClient(cs));

// Register TableClients (typed wrapper instead of anonymous object)
builder.Services.AddSingleton(sp =>
{
    var tsc = sp.GetRequiredService<TableServiceClient>();
    return new TableClients(
        tsc.GetTableClient(tableCustomers),
        tsc.GetTableClient(tableProducts)
    );
});

// Blob container
builder.Services.AddSingleton(sp =>
{
    var bsc = sp.GetRequiredService<BlobServiceClient>();
    return bsc.GetBlobContainerClient(blobContainer);
});

// Queue
builder.Services.AddSingleton(sp =>
{
    var qsc = sp.GetRequiredService<QueueServiceClient>();
    return qsc.GetQueueClient(queueName);
});

// File share
builder.Services.AddSingleton(sp =>
{
    var ssc = sp.GetRequiredService<ShareServiceClient>();
    return ssc.GetShareClient(fileShareName);
});

// Register repositories for DI
builder.Services.AddScoped(sp =>
    new TableRepository<CustomerEntity>(
        sp.GetRequiredService<TableClients>().Customers));

builder.Services.AddScoped(sp =>
    new TableRepository<ProductEntity>(
        sp.GetRequiredService<TableClients>().Products));

var app = builder.Build();

// Ensure resources exist (once at startup)
using (var scope = app.Services.CreateScope())
{
    var tables = scope.ServiceProvider.GetRequiredService<TableClients>();
    tables.Customers.CreateIfNotExists();
    tables.Products.CreateIfNotExists();

    var container = scope.ServiceProvider.GetRequiredService<BlobContainerClient>();
    container.CreateIfNotExists();

    var queue = scope.ServiceProvider.GetRequiredService<QueueClient>();
    queue.CreateIfNotExists();

    var share = scope.ServiceProvider.GetRequiredService<ShareClient>();
    share.CreateIfNotExists();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
