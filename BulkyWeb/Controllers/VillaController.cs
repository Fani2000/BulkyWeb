using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize]
    public class VillaController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;

        // private readonly ApplicationDbContext _context;

        /*
        public VillaController(ApplicationDbContext context)
        {
            _context = context;
        }
        */

        public VillaController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            this.unitOfWork = unitOfWork;
            this.webHostEnvironment = webHostEnvironment;
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

            if(obj.Image != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);

                string imagePath = Path.Combine(webHostEnvironment.WebRootPath,@"images\Villa");

                using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                obj.Image.CopyTo(fileStream);

                obj.ImageUrl = @"\images\Villa\" + fileName;

            }else
            {
                obj.ImageUrl = "https://placehold.co/600x400";
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

            if(obj.Image != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                string imagePath = Path.Combine(webHostEnvironment.WebRootPath,@"images\Villa");

                if(!string.IsNullOrEmpty(obj.ImageUrl))
                {
                    var oldImagePath = Path.Combine(webHostEnvironment.WebRootPath , imagePath.TrimStart('\\'));

                    if(System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                obj.Image.CopyTo(fileStream);

                obj.ImageUrl = @"\images\Villa\" + fileName;
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

            if (!string.IsNullOrEmpty(obj.ImageUrl))
            {
                var oldImagePath = Path.Combine(webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            unitOfWork.Villa.Remove(objFromDb);
            unitOfWork.Villa.Save();
            TempData["success"] = "The villa has been delete successfully";
            return RedirectToAction("Index", "Villa");
        }
    }
}
