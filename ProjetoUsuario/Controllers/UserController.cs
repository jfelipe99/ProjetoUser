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
        public ActionResult Create([FromBody]User user)
        {
            // validate that our model meets the requirement
            if (ModelState.IsValid)
            {
                try
                {

                    var returnvalue = _userservices.Insert(user);
                    return Ok(returnvalue);
          
                }
                catch(Exception ex)
                {
                    
                    var error = $"Something went wrong {ex.Message}";
                    return BadRequest(error);
                }
            }else{

                ModelState.AddModelError(string.Empty, $"Something went wrong, invalid model");
                return BadRequest( $"Something went wrong, invalid model");
            }          
        }

        [HttpPut]
        public ActionResult Update([FromBody] User user)
        {
             if (ModelState.IsValid)
            {
                try
                {
                     var returnvalue = _userservices.Update(user);

                     return Ok(returnvalue);
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Something went wrong {ex.Message}");
                    return BadRequest($"Something went wrong {ex.Message}");
                }
            }else {
                ModelState.AddModelError(string.Empty, $"Something went wrong, invalid model");
                    var error = $"Something went wrong, invalid model";
                    return BadRequest(error);
            }
        }

        [HttpDelete]
        public ActionResult Delete([FromBody]User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var returnvalue =_userservices.Delete(user);

                     return Ok(returnvalue);
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Something went wrong {ex.Message}");
                    return BadRequest($"Something went wrong {ex.Message}");
                }
            }else {  
                ModelState.AddModelError(string.Empty, $"Something went wrong, invalid model");
                var error = $"Something went wrong, invalid model";
                return BadRequest(error);
            }
        }
    }
}