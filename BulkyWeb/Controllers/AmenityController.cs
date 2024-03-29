using Application.Common.Interfaces;
using Application.Common.Utility;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers
{
    [Authorize(Roles = SD.ROLE_ADMIN)]
    public class AmenityController : Controller
    {
        private readonly IUnitOfWork unitOfWork;


        public AmenityController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var villaNumbers = unitOfWork.Amenity.GetAll(includeProperties: "Villa").ToList();

            return View(villaNumbers);
        }

        private void LoadVillaListItems ()
        {

            IEnumerable<SelectListItem> list = unitOfWork.Villa.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

            ViewData["AmenityList"] = list;
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
        public async Task<IActionResult> Create(Amenity obj)
        {

            bool isNumberUnique = unitOfWork.Amenity.GetAny(x => x.Id == obj.Id);

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
            unitOfWork.Amenity.Add(obj);
            unitOfWork.Amenity.Save();

            TempData["success"] = "The villa Number has been created successfully";
            return RedirectToAction(nameof(Index), "Amenity");

        }

        public IActionResult Update(int amenityId)
        {
            Amenity? obj = unitOfWork.Amenity.GetVilla(x => x.Id == amenityId);

            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }

            LoadVillaListItems();

            return View(obj);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Amenity obj)
        {

             ModelState.Remove("Villa"); // Removes the prop you don't want to validate from your model

            if (!ModelState.IsValid)
            {
                TempData["error"] = "The villa Number couldn't be created.";
                return View(obj);
            }

            unitOfWork.Amenity.Update(obj);
            unitOfWork.Amenity.Save();

            TempData["success"] = "The villa Number has been updated successfully";
            return RedirectToAction(nameof(Index), "Amenity");
        }


        public IActionResult Delete(int amenityId)
        {
            Amenity? obj = unitOfWork.Amenity.GetVilla(x => x.Id == amenityId);

            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }

            LoadVillaListItems();

            return View(obj);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Amenity obj)
        {

            Amenity? objFromDb = unitOfWork.Amenity.GetVilla((x => x.Id == obj.Id));

            if (objFromDb is null)
            {
                TempData["error"] = "The villa couldn't be deleted, try again.";
                return View();
            }

            unitOfWork.Amenity.Remove(objFromDb);
            unitOfWork.Amenity.Save();
            TempData["success"] = "The villa has been delete successfully";
            return RedirectToAction(nameof(Index), "Amenity");
        }
    }
}
