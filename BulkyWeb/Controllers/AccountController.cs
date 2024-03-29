using Application.Common.Interfaces;
using Application.Common.Utility;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.Models.ViewModels;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(IUnitOfWork unitOfWork,UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        { 
            _signInManager = signInManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Login(string returnUrl=null)
        {
            returnUrl ??= Url.Content("~/");

            LoginVm loginVM = new()
            {
                RedirectUrl = returnUrl
            };

            return View(loginVM);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        private async Task CreateRoles()
        {
            if (!(await _roleManager.RoleExistsAsync(SD.ROLE_ADMIN)))
            {
                await _roleManager.CreateAsync(new IdentityRole(SD.ROLE_ADMIN));
                await _roleManager.CreateAsync(new IdentityRole(SD.ROLE_CUSTOMER));
                await _roleManager.CreateAsync(new IdentityRole(SD.ROLE_Owner));
            }
        }

        private IEnumerable<SelectListItem> getRoles()
        {
            return _roleManager.Roles.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Name
            });
        }
        public IActionResult Register(string returnUrl=null)
        {
            returnUrl ??= Url.Content("~/");

            CreateRoles().Wait();

            RegistrationVm registrationVM = new()
            {
                RoleList = getRoles(),
                RedirectUrl = returnUrl
            };

            return View(registrationVM);
        }

        private IActionResult safeRedirect(string? redirectUrl)
        {
            if (string.IsNullOrEmpty(redirectUrl))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return LocalRedirect(redirectUrl);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationVm registerVm)
        {

            if (ModelState.IsValid)
            {

            ApplicationUser user = new()
            {
                Name = registerVm.Name,
                Email = registerVm.Email,
                PhoneNumber = registerVm.PhoneNumber,
                UserName = registerVm.Email,
                EmailConfirmed = true,
                CreatedAt = DateTime.Now,
                NormalizedEmail = registerVm.Email.ToUpper()
            };

            var result = await _userManager.CreateAsync(user, registerVm.Password);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(registerVm.Role))
                {
                    await _userManager.AddToRoleAsync(user, registerVm.Role);
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, SD.ROLE_CUSTOMER);
                }

                await _signInManager.SignInAsync(user, isPersistent: false);

                // return RedirectToAction("Index", "Home");
                return safeRedirect(registerVm.RedirectUrl);
            }

            foreach (var error in result.Errors)
            {
               ModelState.AddModelError("", error.Description); 
            }
            }

            registerVm.RoleList = getRoles();

            return View(registerVm);
        }

        
        [HttpPost]
        public async Task<IActionResult> Login(LoginVm loginVm)
        {


            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginVm.Email, loginVm.Password,
                    loginVm.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return safeRedirect(loginVm.RedirectUrl);
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt");
            }

            return View(loginVm);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
