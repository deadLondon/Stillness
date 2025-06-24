using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Stillness.Data;
using Stillness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace Stillness.Controllers
{
    public class UserController : Controller
    {
        private readonly StillnessContext _context;

        public UserController(StillnessContext context)
        {
            _context = context;
        }

        // GET: User
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            int? userId = HttpContext.Session.GetInt32("LoggedId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            User user = await _context.User.FindAsync((int)userId);
            var pictures = await _context.Pictures
                .Where(p=>p.UserId == userId)
                .ToListAsync();
            var viewModel = new UserPictureViewModel 
            {
                User = user,
                Pictures = pictures
            };

            return View(viewModel);
        }


        // GET: User/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user,IFormFile Avatar)
        {
            var fromDb = await _context.User.AnyAsync(u => u.Nickname == user.Nickname);
            if (user == null || fromDb == true)
            {
                return View();
            }
            using (MemoryStream ms = new MemoryStream())
            {
                Avatar.CopyTo(ms);
                string base64 = Convert.ToBase64String(ms.ToArray());
                user.Avatar = base64;
            }
            user.CreatedAt = DateTime.Now.Date;
            _context.User.Add(user);
            _context.SaveChanges();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        //To do : exceptions for all methods!
        [HttpPost]
        public async Task<IActionResult> Login(string Nickname , string Password)
        {
            User fromDb = await _context.User.FirstOrDefaultAsync(u => u.Nickname == Nickname && u.Password == Password);
            if (fromDb != null)
            {
                HttpContext.Session.SetInt32("LoggedId",fromDb.Id);
                HttpContext.Session.SetString("Pfp", fromDb.Avatar);
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
