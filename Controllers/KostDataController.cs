using BoardingHouseApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

namespace BoardingHouseApp.Controllers
{
    public class KostDataController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public KostDataController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Detail()
        {
            List<KostData> kost = _context.KostData.ToList();
            return View(kost);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] KostForm data, IFormFile Photo)
        {
            if (!ModelState.IsValid)
            {
                return View(data);
            }

            var kostdata = new KostData()
            {
                Name = data.Name,
                Address = data.Address,
                Price = data.Price,
                Room = data.Room,
            };

            if (Photo != null)
            {
                if (Photo.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var fileExt = Path.GetExtension(Photo.FileName).ToLower();
                    if (!allowedExtensions.Contains(fileExt))
                    {
                        ModelState.AddModelError("Foto", "File type is not allowed. Please upload a JPG or PNG file.");
                        return View(data);
                    }

                    var fileFolder = Path.Combine(_env.WebRootPath, "Kost");

                    if (!Directory.Exists(fileFolder))
                    {
                        Directory.CreateDirectory(fileFolder);
                    }

                    var fileName = "photo_" + data.Name + Path.GetExtension(Photo.FileName);
                    var fullFilePath = Path.Combine(fileFolder, fileName);
                    using (var stream = new FileStream(fullFilePath, FileMode.Create))
                    {
                        await Photo.CopyToAsync(stream);
                    }

                    kostdata.Photo = fileName;
                }
            }
            _context.KostData.Add(kostdata);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Kost berhasil ditambahkan.";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var kost = _context.KostData.FirstOrDefault(y => y.Id == id);
            if (kost == null)
            {
                return NotFound();
            }

            return View(kost);
        }

        [HttpPost]
        public IActionResult Edit([FromForm] KostData kosts)
        {
            var eKost = _context.KostData.FirstOrDefault(y => y.Id == kosts.Id);
            if (eKost != null)
            {
                eKost.Name = kosts.Name;
                eKost.Address = kosts.Address;
                eKost.Price = kosts.Price;
                eKost.Room = kosts.Room;

                _context.KostData.Update(eKost);
                _context.SaveChanges();
            }
            return RedirectToAction("Index", "KostData");
        }


        public IActionResult Delete(int id)
        {
            var kost = _context.KostData.FirstOrDefault(x => x.Id == id);

            _context.KostData.Remove(kost);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            var kosts = _context.KostData.ToList();
            return View(kosts);
        }

        [HttpPost]
		public async Task<IActionResult> Search(string name, decimal minPrice, decimal maxPrice)
		{
			var kosts = await _context.KostData
				.Where(k => k.Name.Contains(name) || k.Price >= minPrice && k.Price <= maxPrice)
				.ToListAsync();

			return View("SearchResult", kosts);
		}
	}
}
