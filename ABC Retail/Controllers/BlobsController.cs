using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;

public class BlobsController : Controller
{
    private readonly BlobContainerClient _container;
    public BlobsController(BlobContainerClient container) => _container = container;

    public async Task<IActionResult> Index()
    {
        var items = new List<string>();
        await foreach (var blob in _container.GetBlobsAsync())
            items.Add(blob.Name);
        return View(items);
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file is { Length: > 0 })
        {
            var client = _container.GetBlobClient(file.FileName);
            await using var s = file.OpenReadStream();

            // Overwrite if exists
            await client.UploadAsync(s, overwrite: true);

            // Set content type after upload
            await client.SetHttpHeadersAsync(new BlobHttpHeaders
            {
                ContentType = file.ContentType
            });
        }
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Download(string name)
        => Redirect(_container.GetBlobClient(name).Uri.ToString());
}
