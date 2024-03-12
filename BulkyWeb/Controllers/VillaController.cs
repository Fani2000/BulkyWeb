using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VillaController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var villas = _context.Villas.ToList();

            return View(villas);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Villa obj)
        {

            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            _context.Villas.Add(obj);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Villa");

        }
    }
}
