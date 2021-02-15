using Domain.Models;
using IdentityServer.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthController(SignInManager<AppUser> signInManager,
                              UserManager<AppUser> userManager,
                              IHttpClientFactory httpClientFactory)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            //validate view model
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var result = await _signInManager.PasswordSignInAsync(viewModel.Username, viewModel.Password, false, false);

            if (result.Succeeded)
            {
                return Redirect(viewModel.ReturnUrl);
            }

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            return View(new RegisterViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var newUser = new AppUser()
            {
                UserName = viewModel.Username,
                DisplayName = viewModel.DisplayName
            };

            var result = await _userManager.CreateAsync(newUser, viewModel.Password);

            if (result.Succeeded)
            {
                //add displayname in our id_token
                await _userManager.AddClaimAsync(newUser, new Claim("DisplayName", newUser.DisplayName));
                await _signInManager.SignInAsync(newUser, false);

                //redirect back to our servers callback
                return Redirect(viewModel.ReturnUrl);

            }

            return View(viewModel);
        }
    }
}