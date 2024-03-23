using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        // private readonly ApplicationDbContext _context;

        /*
        public VillaController(ApplicationDbContext context)
        {
            _context = context;
        }
        */

        public VillaController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var villas = unitOfWork.Villa.GetAll();

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

            unitOfWork.Villa.Add(obj);
            unitOfWork.Villa.Save();

            TempData["success"] = "The villa has been created successfully";
            return RedirectToAction("Index", "Villa");

        }

        public IActionResult Update(int villaId)
        {
            Villa? obj = unitOfWork.Villa.GetVilla(x => x.Id == villaId);

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

            unitOfWork.Villa.Update(obj);
            unitOfWork.Villa.Save();
            TempData["success"] = "The villa has been updated successfully";
            return RedirectToAction("Index", "Villa");
        }


        public IActionResult Delete(int villaId)
        {
            Villa? obj = unitOfWork.Villa.GetVilla(x => x.Id == villaId);

            if(obj == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(obj);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Villa obj)
        {
            Villa? objFromDb = unitOfWork.Villa.GetVilla(x => x.Id == obj.Id);

            if (objFromDb is null)
            {
                TempData["error"] = "The villa couldn't be deleted, try again.";
                return View();
            }

            unitOfWork.Villa.Remove(objFromDb);
            unitOfWork.Villa.Save();
            TempData["success"] = "The villa has been delete successfully";
            return RedirectToAction("Index", "Villa");
        }
    }
}
