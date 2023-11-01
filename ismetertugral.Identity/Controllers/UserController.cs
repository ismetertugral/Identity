using ismetertugral.Identity.Context;
using ismetertugral.Identity.Entities;
using ismetertugral.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ismetertugral.Identity.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IEContext _context;
        public UserController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IEContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            //var query = _userManager.Users;
            //var users = _context.Users.Join(_context.UserRoles, user => user.Id, userRole => userRole.UserId, (user, userRole) => new
            //{
            //    user,
            //    userRole
            //}).Join(_context.Roles, two => two.userRole.RoleId, role => role.Id, (two, role) => new
            //{
            //    two.user,
            //    two.userRole,
            //    role
            //}).Where(x => x.role.Name != "Admin").Select(x => new AppUser
            //{
            //    Id = x.user.Id,
            //    AccessFailedCount = x.user.AccessFailedCount,
            //    ConcurrencyStamp = x.user.ConcurrencyStamp,
            //    Email = x.user.Email,
            //    EmailConfirmed = x.user.EmailConfirmed,
            //    Gender = x.user.Gender,
            //    ImagePath = x.user.ImagePath,
            //    LockoutEnabled = x.user.LockoutEnabled,
            //    LockoutEnd = x.user.LockoutEnd,
            //    NormalizedEmail = x.user.NormalizedEmail,
            //    NormalizedUserName = x.user.NormalizedUserName,
            //    PasswordHash = x.user.PasswordHash,
            //    PhoneNumber = x.user.PhoneNumber,
            //    UserName = x.user.UserName,
            //}).ToList();

            //ismet ERTUGRAL
            //var users = _context.Users
            //  .Where(z => !(_context.UserRoles.Join(_context.Roles, userRole => userRole.RoleId, role => role.Id, (userRole, role) => new
            //  {
            //      userRole.UserId,
            //      role
            //  }).Where(x => x.role.Name == "Admin").Select(x => x.UserId).Contains(z.Id))).ToList();

            //var users = await _userManager.GetUsersInRoleAsync("Member");
            //return View(users);

            List<AppUser> filteredUser = new List<AppUser>();

            var users = _context.Users.ToList();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (!roles.Contains("Admin"))
                    filteredUser.Add(user);
            }

            return View(filteredUser);
        }

        public IActionResult Create()
        {
            return View(new UserCreateAdminModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateAdminModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    Gender = model.Gender,
                };
                var result = await _userManager.CreateAsync(user, user.UserName + "1234");

                var memberRole = await _roleManager.FindByNameAsync("Member");
                if (memberRole == null)
                    await _roleManager.CreateAsync(new()
                    {
                        Name = "Member",
                        CreatedTime = DateTime.UtcNow,
                    });
                await _userManager.AddToRoleAsync(user, "Member");

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> AssignRoles(int id)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == id);
            var userRoles = await _userManager.GetRolesAsync(user);
            var Roles = _roleManager.Roles.ToList();

            RoleAssignSendModel model = new RoleAssignSendModel();

            List<RoleAssignListModel> list = new List<RoleAssignListModel>();

            foreach (var role in Roles)
            {
                list.Add(new RoleAssignListModel
                {
                    Name = role.Name,
                    RoleId = role.Id,
                    Exist = userRoles.Contains(role.Name)
                });
            }

            model.UserId = id;
            model.Roles = list;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRoles(RoleAssignSendModel model)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == model.UserId);
            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in model.Roles)
            {
                if (role.Exist)
                {
                    if (!userRoles.Contains(role.Name))
                        await _userManager.AddToRoleAsync(user, role.Name);
                }
                else
                {
                    if (userRoles.Contains(role.Name))
                        await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
            }
            return RedirectToAction("Index");
        }
    }
}
