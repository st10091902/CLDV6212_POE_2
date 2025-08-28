using ABC_Retail.Models;
using ABC_Retail.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABC_Retail.Controllers
{
    public class ProductsController : Controller
    {
        private readonly TableRepository<ProductEntity> _repo;

        public ProductsController(TableRepository<ProductEntity> repo) => _repo = repo;

        // GET: /Products
        public async Task<IActionResult> Index()
            => View(await _repo.ListAsync("PRODUCT"));

        // GET: /Products/Create
        [HttpGet]
        public IActionResult Create() => View(new ProductEntity());

        // POST: /Products/Create
        [HttpPost]
        public async Task<IActionResult> Create(ProductEntity model)
        {
            model.PartitionKey = "PRODUCT";
            await _repo.AddAsync(model);
            return RedirectToAction(nameof(Index));
        }
    }
}
