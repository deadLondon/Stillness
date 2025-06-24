using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using Stillness.Data;
using Stillness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Stillness.Controllers
{
    public class PicturesController : Controller
    {
        private readonly StillnessContext _context;
        private readonly IWebHostEnvironment env;


        public PicturesController(StillnessContext context , IWebHostEnvironment env)
        {
            _context = context;
            this.env = env;
        }

        // GET: Pictures
        public IActionResult Index(string pattern)
        {
            var pictures = _context.Pictures.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pattern))
            {
                if (pattern.Contains("*") || pattern.Contains("?"))
                {
                    var regexPattern = "^" + Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", ".") + "$";
                    var regex = new Regex(regexPattern, RegexOptions.IgnoreCase);

                    pictures = pictures
                        .AsEnumerable()
                        .Where(p =>
                        {
                            var fileName = Path.GetFileNameWithoutExtension(p.Filename);
                            var namePart = fileName.Contains('_') ? fileName[(fileName.IndexOf('_') + 1)..] : fileName;
                            return regex.IsMatch(namePart);
                        })
                        .AsQueryable();
                }
                else
                {
                    pattern = pattern.ToLower();
                    pictures = pictures
                        .AsEnumerable()
                        .Where(p =>
                        {
                            var fileName = Path.GetFileNameWithoutExtension(p.Filename);
                            var namePart = fileName.Contains('_') ? fileName[(fileName.IndexOf('_') + 1)..] : fileName;
                            return namePart.ToLower().Contains(pattern);
                        })
                        .AsQueryable();
                }
            }

            var result = pictures.ToList();
            return View(result);
        }


        // GET: Pictures/Create
        public IActionResult Create()
        {
            int? userId = HttpContext.Session.GetInt32("LoggedId");
            if (userId == null)
            {
                return RedirectToAction("Login","User");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Pictures picture , IFormFile Filename)
        {
            int? uid = HttpContext.Session.GetInt32("LoggedId");
            if (Filename != null || uid != null)
            {
                string fName = DateTime.Now.ToString("yyyyMMddHHmmssffffff") + "_" + Filename.FileName;
                string path = Path.Combine(env.WebRootPath, "images", fName);
                string relativePath = "images/" + fName; 

                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    Filename.CopyTo(fs);
                }

                picture.UserId = uid != null ? (int)uid : 0;
                picture.Filename = fName;
                picture.CreatedAt = DateTime.Now;
                picture.Filepath = relativePath;

                _context.Pictures.Add(picture);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet("/id/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            Pictures ?pic = await _context.Pictures.FindAsync((int)id);
            var user = await _context.User
                .Where(u => u.Id == pic.UserId)
                .FirstOrDefaultAsync();
            var viewModel = new UserPictureViewModel
            {
                User = user,
                Pictures = new List<Pictures> { pic }
            };
            return View(viewModel);
        }

        public FileResult DownloadPicture (string pic)
        {
            string[] fileName = pic.Split("/");
            byte[] data = System.IO.File.ReadAllBytes(Path.Combine(env.WebRootPath, "images", fileName[1]));
            FileContentResult fc = File(data, MediaTypeNames.Application.Octet, fileName[1]);
            return fc;
        }

    }
}
