using BoardingHouseApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Linq;

namespace BoardingHouseApp.Controllers
{
    public class BookingDataController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BookingDataController(AppDbContext c, IWebHostEnvironment x)
        {
            _context = c;
            _env = x;
        }

        public IActionResult Edit(int id)
        {
            var bookings = _context.BookingDates.FirstOrDefault(y => y.Id == id);
            if (bookings == null)
            {
                return NotFound();
            }

            return View(bookings);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromForm] BookingData booking)
        {
            var ebook = _context.BookingDates.FirstOrDefault(y => y.Id == booking.Id);
            if (ebook != null)
            {
                ebook.Name = booking.Name;
                ebook.PhoneNumber = booking.PhoneNumber;
                ebook.IDCardNumber = booking.IDCardNumber;
                ebook.Work = booking.Work;
                ebook.Count = booking.Count;
                _context.BookingDates.Update(ebook);
                _context.SaveChanges();
            }
            return RedirectToAction("Index", "BookingData");
        }

        public IActionResult Delete(int id)
        {
            var booking = _context.BookingDates.FirstOrDefault(x => x.Id == id);

            _context.BookingDates.Remove(booking);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        /*public IActionResult Create(int id)
        {
            var getIdKostData = _context.KostData.Where(dk => dk.Id == 1).FirstOrDefault();
            Console.WriteLine("test "+id);
            return View(getIdKostData);
        }*/

        public IActionResult Create()
        {
            var kosts = _context.KostData.ToList();
            return View(kosts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] BookingForm dataBooking)
        {
            try
            {
                var cekKost = await _context.KostData.FirstOrDefaultAsync(b => b.Id == dataBooking.DataKost);
                if (cekKost == null)
                {
                    return NotFound("Kost not found.");
                }

                var bookingData = new BookingData
                {
                    Name = dataBooking.Name,
                    PhoneNumber = dataBooking.PhoneNumber,
                    IDCardNumber = dataBooking.IDCardNumber,
                    Work = dataBooking.Work,
                    Count = dataBooking.Count,
                    KostData = cekKost
                };

                _context.BookingDates.Add(bookingData);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Booking berhasil ditambahkan.";
                Console.WriteLine("test");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction("Error", "Home");
            }
        } 
        public IActionResult Index()
        {
            var bookings = _context.BookingDates.ToList();
            return View(bookings);
        }
    }
}