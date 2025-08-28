using Azure.Storage.Queues;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

public class QueueController : Controller
{
    private readonly QueueClient _queue;
    public QueueController(QueueClient queue) => _queue = queue;

    [HttpGet] public IActionResult Index() => View();

    [HttpPost]
    public async Task<IActionResult> Enqueue(string orderId, string customerId)
    {
        var msg = JsonSerializer.Serialize(new { orderId, customerId, status = "Processing" });
        await _queue.SendMessageAsync(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(msg)));
        ViewBag.Message = "Order enqueued.";
        return View("Index");
    }
}
