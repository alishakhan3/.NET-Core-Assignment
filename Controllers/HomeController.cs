using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Areas.Identity.Data;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Service;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserService _userService;
        private readonly WebDbContext _context;
        private readonly UserManager<WebApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, UserService userService, WebDbContext context, UserManager<WebApplicationUser> userManager)
        {
            _logger = logger;
            _userService = userService;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var profileLinks = await GetProfileLinks();
            return View(profileLinks);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePersonalData(string newEmail, string newName, string newAddress, string newPassword)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user != null)
            {
                user.Email = newEmail;
                user.Name = newName;
                user.Address = newAddress;

                if (!string.IsNullOrEmpty(newPassword))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    await _userManager.ResetPasswordAsync(user, token, newPassword);
                }

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }

            TempData["ErrorMessage"] = "Failed to update personal data";
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> AddProfile(string accId, string profileType, string name, string url)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _userService.AddUserProfileAsync(userId, accId, profileType, name, url);

                return RedirectToAction("Index");
            }
            catch (ArgumentException)
            {
                Debug.WriteLine("Key Already In Use");
                return RedirectToAction("Index"); 
            }
        }


        public async Task<List<UserProfile>> GetProfileLinks()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userProfiles = await _userService.GetUserProfile(userId);

            Debug.WriteLine($"Home- GetProfileLink- UserProfiles count: {userProfiles.Count}");

            return userProfiles;
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProfile(string accId)
        {
            var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(up => up.accId == accId);
            if (userProfile != null)
            {
                _context.UserProfiles.Remove(userProfile);
                await _context.SaveChangesAsync();
                return Ok();
            }

            return NotFound();
        }

        public async Task<IActionResult> EditProfile(string accId, string profileType, string name, string url)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _userService.UpdateUserProfileAsync(userId, accId, profileType, name, url);

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
