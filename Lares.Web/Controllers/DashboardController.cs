using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Lares.Entities;
using Lares.Infrastructure;
using Lares.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Lares.Controllers
{
    public class DashboardController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardController(DataContext dataContext, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = dataContext;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        /*
         * Returns the Id of the current User
         */
        private string getCurrentUserId()
        {
            return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        private async Task<User> getCurrentUser()
        {
            return await _userManager.FindByIdAsync(getCurrentUserId());
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var currentUser = getCurrentUser().Result;
            var currentProperty = await _context.Property.FindAsync(currentUser.SelectedPropertyId);

            DashboardViewModel dashboardViewModel = new DashboardViewModel();

            dashboardViewModel.CurrentUser = currentUser;
            dashboardViewModel.CurrentProperty = currentProperty;

            return View(dashboardViewModel);
        }
    }
}
