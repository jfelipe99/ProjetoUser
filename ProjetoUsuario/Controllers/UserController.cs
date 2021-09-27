using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjetoUsuario.Data;
using ProjetoUsuario.Models;

namespace ProjetoUsuario.Controllers
{
    [Route("api/UserController")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserDbContext _context;
        private IUserServices _userservices;
        public UserController(IUserServices userServices)
        {
            _userservices = userServices;
        }

        public IActionResult Index()
        {
            var books = _context.Users.ToList();
            return View(books);
        }

        [HttpGet]
        [Route("")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public int Create([FromBody]User user)
        {
            // validate that our model meets the requirement
            if (ModelState.IsValid)
            {
                try
                {
                    // update the ef core context in memory 
                    var returnvalue = _userservices.Insert(user);
                    return returnvalue;
                    // sync the changes of ef code in memory with the database                
                }
                catch(Exception ex)
                {
                    
                    ModelState.AddModelError(string.Empty, $"Something went wrong {ex.Message}");
                    return 153;
                }
            }           

            ModelState.AddModelError(string.Empty, $"Something went wrong, invalid model");
            return 3;
            // We return the object back to view
            
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] User user)
        {
             if (ModelState.IsValid)
            {
                try
                {
                    // update the ef core context in memory 
                    var returnvalue =_userservices.Update(user);

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
        public async Task<IActionResult> Delete([FromBody]User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // update the ef core context in memory 
                    var returnvalue =_userservices.Delete(user);

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