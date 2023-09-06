using Code2.Data;
using Code2.Models;
using Code2.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Code2.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdminController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly UserManager<User> _userManager;

		public AdminController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
		{
			_context = context;
			_roleManager = roleManager;
			_userManager = userManager;
		}

		[HttpGet]
		public IActionResult ListAllRoles()
		{
			var roles = _roleManager.Roles;
			return View(roles);
		}
		[HttpGet]
		public IActionResult ListAllUsers()
		{
			var users = _userManager.Users;
			return View(users);
		}


		[HttpGet]
		public IActionResult AddRole()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> AddRole(AddRoleViewModel model)
		{
			if (ModelState.IsValid)
			{
				IdentityRole identityRole = new()
				{
					Name = model.RoleName
				};

				var result = await _roleManager.CreateAsync(identityRole);

				if (result.Succeeded)
				{
					return RedirectToAction("ListAllRoles");
				}

				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
			}
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> EditRole(string id)
		{
			var role = await _roleManager.FindByIdAsync(id);

			if (role == null)
			{
				ViewData["ErrorMessage"] = $"No role with Id '{id}' was found";
				return View("Error");
			}

			EditRoleViewModel model = new()
			{
				Id = role.Id,
				RoleName = role.Name,
			};
			var list = _userManager.Users.ToList();
			foreach (var user in list)
			{
				if (await _userManager.IsInRoleAsync(user, role.Name))
				{
					model.Users.Add(user.UserName);
				}

			}
			return View(model);
		}


		[HttpPost]
		public async Task<IActionResult> EditRole(EditRoleViewModel model)
		{
			var role = await _roleManager.FindByIdAsync(model.Id);

			if (role == null)
			{
				ViewData["ErrorMessage"] = $"No role with Id '{model.Id}' was found";
				return View("Error");
			}
			else
			{
				role.Name = model.RoleName;

				var result = await _roleManager.UpdateAsync(role);

				if (result.Succeeded)
				{
					return RedirectToAction("ListAllRoles");
				}

				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}

				return View(model);

			}
		}
		[HttpGet]
		public async Task<IActionResult> EditUsersInRole(string id)
		{
			var role = await _roleManager.FindByIdAsync(id);

			ViewData["roleId"] = id;
			ViewData["roleName"] = role.Name;

			if (role == null)
			{
				ViewData["ErrorMessage"] = $"No role with Id '{id}' was found";
				return View("Error");
			}
			var model = new List<UserRoleViewModel>();
			var list = _userManager.Users.ToList();
			foreach (var user in list)
			{
				UserRoleViewModel userRoleVM = new()
				{
					Id = user.Id,
					Name = user.UserName
				};
				if (await _userManager.IsInRoleAsync(user, role.Name))
				{
					userRoleVM.IsSelected = true;
				}
				else
				{
					userRoleVM.IsSelected = false;
				}
				model.Add(userRoleVM);
			}
			return View(model);
		}
		[HttpPost]
		public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string id)
		{
			var role = await _roleManager.FindByIdAsync(id);

			if (role == null)
			{
				ViewData["ErrorMessage"] = $"No role with Id '{id}' was found";
				return View("Error");
			}
			for (int i = 0; i < model.Count; i++)
			{
				var user = await _userManager.FindByIdAsync(model[i].Id);
				if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
				{
					await _userManager.AddToRoleAsync(user, role.Name);
				}
				else if (!model[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
				{
					await _userManager.RemoveFromRoleAsync(user, role.Name);
				}
				else
				{
					continue;
				}
			}
			return RedirectToAction("EditRole", new { Id = id });
		}
		[HttpGet]
		public async Task<IActionResult> DeleteRole(string id)
		{
			var role = await _roleManager.FindByIdAsync(id);

			return View(role);
		}
		[HttpPost]
		public async Task<IActionResult> ConfirmDelete(string id)
		{
			var role = await _roleManager.FindByIdAsync(id);

			if (role == null)
			{
				ViewData["ErrorMessage"] = $"No role with Id '{id}' was found";
				return View("Error");
			}
			else
			{
				var result = await _roleManager.DeleteAsync(role);

				if (result.Succeeded)
				{
					return RedirectToAction("ListAllRoles");
				}
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
				return View(role);
			}
		}
		[HttpGet]
		public async Task<IActionResult> DeleteUser(string id)
		{
			var user = await _userManager.FindByIdAsync(id);

			return View(user);
		}
		[HttpPost]
		public async Task<IActionResult> ConfirmDeleteUser(string id)
		{
			var user = await _userManager.FindByIdAsync(id);

			if (user == null)
			{
				ViewData["ErrorMessage"] = $"No user with Id '{id}' was found";
				return View("Error");
			}
			else
			{
				var result = await _userManager.DeleteAsync(user);

				if (result.Succeeded)
				{
					return RedirectToAction("ListAllUsers");
				}
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
				return View(user);
			}
		}
		[HttpGet]
		public IActionResult ResetPassword(string id)
		{
			var model = new ResetPasswordViewModel { UserId = id };
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByIdAsync(model.UserId);
				if (user != null)
				{
					var result = await _userManager.RemovePasswordAsync(user);
					if (result.Succeeded)
					{
						result = await _userManager.AddPasswordAsync(user, model.NewPassword);
						if (result.Succeeded)
						{
							return RedirectToAction("ListAllUsers");
						}
					}

					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}
				else
				{
					ModelState.AddModelError(string.Empty, "User not found");
				}
			}

			return View(model);
		}


	}
}

