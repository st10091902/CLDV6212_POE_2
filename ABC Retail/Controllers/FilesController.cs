using Azure;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using Microsoft.AspNetCore.Mvc;

public class FilesController : Controller
{
    private readonly ShareClient _share;
    public FilesController(ShareClient share) => _share = share;

    public async Task<IActionResult> Index()
    {
        var dir = _share.GetRootDirectoryClient();
        var names = new List<string>();
        await foreach (var item in dir.GetFilesAndDirectoriesAsync())
            if (!item.IsDirectory) names.Add(item.Name);
        return View(names);
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file is { Length: > 0 })
        {
            var dir = _share.GetRootDirectoryClient();
            var fc = dir.GetFileClient(file.FileName);

            await fc.CreateAsync(file.Length);
            await using var s = file.OpenReadStream();
            await fc.UploadRangeAsync(new HttpRange(0, file.Length), s);

            // set Content-Type (new API shape)
            await fc.SetHttpHeadersAsync(new ShareFileSetHttpHeadersOptions
            {
                HttpHeaders = new ShareFileHttpHeaders
                {
                    ContentType = file.ContentType
                }
            });
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Download(string name)
    {
        var dir = _share.GetRootDirectoryClient();
        var fc = dir.GetFileClient(name);

        var download = await fc.DownloadAsync();
        var stream = download.Value.Content;

        string contentType = "application/octet-stream";

        return File(stream, contentType, name);
    }

}
