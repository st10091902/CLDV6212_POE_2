namespace ABC_Retail.Controllers
{
    using ABC_Retail.Models;
    using ABC_Retail.Services;
    using Microsoft.AspNetCore.Mvc;

    public class CustomersController : Controller
    {
        private readonly TableRepository<CustomerEntity> _repo;
        public CustomersController(TableRepository<CustomerEntity> repo) => _repo = repo;

        public async Task<IActionResult> Index()
            => View(await _repo.ListAsync("CUSTOMER"));

        [HttpGet]
        public IActionResult Create() => View(new CustomerEntity());

        [HttpPost]
        public async Task<IActionResult> Create(CustomerEntity model)
        {
            model.PartitionKey = "CUSTOMER";
            await _repo.AddAsync(model);
            return RedirectToAction(nameof(Index));
        }
    }
}
