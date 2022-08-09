using keef2.Models;
using Keefa1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace keef2.Controllers
{
    public class HomeController : Controller
    {
        private readonly DocContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, DocContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var doctor = _context.Doctors.Include(n=>n.depts).OrderByDescending(n => n.id).ToList();
            ViewBag.docs = _context.Doctors.Include(n => n.depts).OrderByDescending(n => n.offerDate).ToList();
            return View(doctor);
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
    }
}
