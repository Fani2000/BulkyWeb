using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IUnitOfWork unitOfWork;


        public VillaNumberController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var villaNumbers = unitOfWork.VillaNumber.GetAll(includeProperties: "Villa").ToList();

            return View(villaNumbers);
        }

        private void LoadVillaListItems ()
        {

            IEnumerable<SelectListItem> list = unitOfWork.Villa.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

            ViewData["VillaList"] = list;
        }

        public async Task<IActionResult> Create()
        {
            // var villaNumbers = unitOfWork.villaNumbers.ToList();
            /* 
            IEnumerable<SelectListItem> list = unitOfWork.Villas.ToList().Select(x => new SelectListItem
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

            bool isNumberUnique = unitOfWork.VillaNumber.GetAny(x => x.Villa_Number == obj.Villa_Number);

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
            unitOfWork.VillaNumber.Add(obj);
            unitOfWork.VillaNumber.Save();

            TempData["success"] = "The villa Number has been created successfully";
            return RedirectToAction(nameof(Index), "VillaNumber");

        }

        public IActionResult Update(int villaNumberId)
        {
            VillaNumber? obj = unitOfWork.VillaNumber.GetVilla(x => x.Villa_Number == villaNumberId);

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

            unitOfWork.VillaNumber.Update(obj);
            unitOfWork.VillaNumber.Save();

            TempData["success"] = "The villa Number has been updated successfully";
            return RedirectToAction(nameof(Index), "VillaNumber");
        }


        public IActionResult Delete(int villaNumberId)
        {
            VillaNumber? obj = unitOfWork.VillaNumber.GetVilla(x => x.Villa_Number == villaNumberId);

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

            VillaNumber? objFromDb = unitOfWork.VillaNumber.GetVilla(x => x.Villa_Number == obj.Villa_Number);

            if (objFromDb is null)
            {
                TempData["error"] = "The villa couldn't be deleted, try again.";
                return View();
            }

            unitOfWork.VillaNumber.Remove(objFromDb);
            unitOfWork.VillaNumber.Save();
            TempData["success"] = "The villa has been delete successfully";
            return RedirectToAction(nameof(Index), "VillaNumber");
        }
    }
}
