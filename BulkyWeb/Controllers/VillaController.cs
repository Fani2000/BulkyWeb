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
                TempData["error"] = "The villa couldn't be created.";
                return View(obj);
            }

            _context.Villas.Add(obj);
            await _context.SaveChangesAsync();

            TempData["success"] = "The villa has been created successfully";
            return RedirectToAction("Index", "Villa");

        }

        public IActionResult Update(int villaId)
        {
            Villa? obj = _context.Villas.FirstOrDefault(x => x.Id == villaId);

            if(obj == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(obj);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Villa obj)
        {

            if (!ModelState.IsValid && obj.Id > 0)
            {
                TempData["error"] = "The Villa couldn't be updated, try agan later.";
                return View(obj);
            }

            _context.Villas.Update(obj);
            await _context.SaveChangesAsync();
            TempData["success"] = "The villa has been updated successfully";
            return RedirectToAction("Index", "Villa");
        }


        public IActionResult Delete(int villaId)
        {
            Villa? obj = _context.Villas.FirstOrDefault(x => x.Id == villaId);

            if(obj == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(obj);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Villa obj)
        {
            Villa? objFromDb = _context.Villas.FirstOrDefault(x => x.Id == obj.Id);

            if (objFromDb is null)
            {
                TempData["error"] = "The villa couldn't be deleted, try again.";
                return View();
            }

            _context.Villas.Remove(objFromDb);
            await _context.SaveChangesAsync();
            TempData["success"] = "The villa has been delete successfully";
            return RedirectToAction("Index", "Villa");
        }
    }
}
