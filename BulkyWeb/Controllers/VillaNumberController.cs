using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VillaNumberController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var villaNumbers = _context.villaNumbers.Include(x => x.Villa).ToList();

            return View(villaNumbers);
        }

        private void LoadVillaListItems ()
        {

            IEnumerable<SelectListItem> list = _context.Villas.ToList().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

            ViewData["VillaList"] = list;
        }

        public async Task<IActionResult> Create()
        {
            // var villaNumbers = _context.villaNumbers.ToList();
            /* 
            IEnumerable<SelectListItem> list = _context.Villas.ToList().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

            ViewData["VillaList"] = list;
            */
            LoadVillaListItems();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VillaNumber obj)
        {

            bool isNumberUnique = _context.villaNumbers.Any(x => x.Villa_Number == obj.Villa_Number);

            ModelState.Remove("Villa"); // Removes the prop you don't want to validate from your model

            if (!ModelState.IsValid)
            {
                TempData["error"] = "The villa Number couldn't be created.";
                return View(obj);
            }

            if(isNumberUnique)
            {
                LoadVillaListItems();
                TempData["error"] = "The villa Number already exists.";
                return View(obj);
            }
            _context.villaNumbers.Add(obj);
            await _context.SaveChangesAsync();

            TempData["success"] = "The villa Number has been created successfully";
            return RedirectToAction("Index", "VillaNumber");

        }

        public IActionResult Update(int villaNumberId)
        {
            VillaNumber? obj = _context.villaNumbers.FirstOrDefault(x => x.Villa_Number == villaNumberId);

            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }

            LoadVillaListItems();

            return View(obj);
        }
        [HttpPost]
        public async Task<IActionResult> Update(VillaNumber obj)
        {

            ModelState.Remove("Villa"); // Removes the prop you don't want to validate from your model

            if (!ModelState.IsValid)
            {
                TempData["error"] = "The villa Number couldn't be created.";
                return View(obj);
            }

            _context.villaNumbers.Update(obj);
            await _context.SaveChangesAsync();

            TempData["success"] = "The villa Number has been updated successfully";
            return RedirectToAction("Index", "VillaNumber");
        }


        public IActionResult Delete(int villaNumberId)
        {
            VillaNumber? obj = _context.villaNumbers.FirstOrDefault(x => x.Villa_Number == villaNumberId);

            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }

            LoadVillaListItems();

            return View(obj);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(VillaNumber obj)
        {

            VillaNumber? objFromDb = _context.villaNumbers.FirstOrDefault(x => x.Villa_Number == obj.Villa_Number);

            if (objFromDb is null)
            {
                TempData["error"] = "The villa couldn't be deleted, try again.";
                return View();
            }

            _context.villaNumbers.Remove(objFromDb);
            await _context.SaveChangesAsync();
            TempData["success"] = "The villa has been delete successfully";
            return RedirectToAction("Index", "VillaNumber");
        }
    }
}
