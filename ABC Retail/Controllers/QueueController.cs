using Azure.Storage.Queues;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace ABC_Retail.Controllers
{
    public class QueueController : Controller
    {
        private readonly QueueClient _queue;

        public QueueController(QueueClient queue)
        {
            _queue = queue;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // Enqueue a message
        [HttpPost]
        public async Task<IActionResult> Enqueue(string orderId, string customerId)
        {
            var payload = JsonSerializer.Serialize(new
            {
                orderId,
                customerId,
                status = "Processing",
                createdUtc = DateTime.UtcNow
            });

            var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(payload));
            await _queue.SendMessageAsync(base64);

            ViewBag.Message = "✅ Order enqueued successfully.";
            return View("Index");
        }

        // Dequeue one message and show its contents
        [HttpPost]
        public async Task<IActionResult> DequeueOne()
        {
            var resp = await _queue.ReceiveMessagesAsync(maxMessages: 1);
            var msg = resp.Value.FirstOrDefault();

            if (msg == null)
            {
                ViewBag.Message = "⚠️ The queue is empty.";
                return View("Index");
            }

            var json = Encoding.UTF8.GetString(Convert.FromBase64String(msg.MessageText));

            // Delete the message after processing
            await _queue.DeleteMessageAsync(msg.MessageId, msg.PopReceipt);

            ViewBag.Message = $"📥 Dequeued message: {json}";
            return View("Index");
        }
    }
}
