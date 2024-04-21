using BoardingHouseApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;

namespace BoardingHouseApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()

		//public IActionResult Index(string search)
		{
			/*var datakost = _context.KostData.Include(d => d.Stat).Where(dk => dk.Stat.Stat == "Published").ToList();

            if (!String.IsNullOrEmpty(search))
            {
                datakost = _context.KostData.Where(x => x.Address.Contains(search)).ToList();
            }
            return View(datakost);*/
			return View();
		}
        public IActionResult Detail(int id)
        {
            KostData kostdata = _context.KostData.Where(x => x.Id == id).FirstOrDefault();
            return View(kostdata);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Admin()
        {
            return View();
        }
    }
}
