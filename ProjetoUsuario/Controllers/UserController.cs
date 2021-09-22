using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjetoUsuario.Data;
using ProjetoUsuario.Models;

namespace ProjetoUsuario.Controllers
{
    public class UserController : Controller
    {
        private readonly UserDbContext _context;

        public UserController(UserDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var books = _context.Users.ToList();
            return View(books);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            // validate that our model meets the requirement
            if (ModelState.IsValid)
            {
                try
                {
                    // update the ef core context in memory 
                    _context.Users.Add(user);

                    // sync the changes of ef code in memory with the database
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Something went wrong {ex.Message}");
                }
            }

            ModelState.AddModelError(string.Empty, $"Something went wrong, invalid model");

            // We return the object back to view
            return View(user);
        }

        [HttpPut]
        public async Task<IActionResult> Update(User user)
        {
             if (ModelState.IsValid)
            {
                try
                {
                    // update the ef core context in memory 
                    _context.Users.Update(user);

                    // sync the changes of ef code in memory with the database
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Something went wrong {ex.Message}");
                }
            }
            ModelState.AddModelError(string.Empty, $"Something went wrong, invalid model");

            // We return the object back to view
            return View(user);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // update the ef core context in memory 
                    _context.Users.Remove(user);

                    // sync the changes of ef code in memory with the database
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Something went wrong {ex.Message}");
                }
            }
            ModelState.AddModelError(string.Empty, $"Something went wrong, invalid model");
            // We return the object back to view
            return View(user);
        }
    }
}