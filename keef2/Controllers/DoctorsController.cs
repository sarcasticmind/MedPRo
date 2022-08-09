using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Keefa1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using keef2.ViewModels;
using Microsoft.AspNetCore.Cors;

namespace keef2.Controllers
{

   [EnableCors("mypolicy")]
 
    public class DoctorsController : Controller
    {
        private readonly DocContext _context;
        private readonly IWebHostEnvironment webhost;

        public DoctorsController(DocContext context , IWebHostEnvironment _webhost)
        {
            _context = context;
            webhost = _webhost;
        }

        #region APIs
        //GetAll 
        //http://localhost:8276/Doctors/GetAll
        [HttpGet]
        public ActionResult GetAll()
        {
            var obj = _context.Doctors.Include(n => n.depts).Select(n => new Doctor() {
                id=n.id,
                fName = n.fName,
                lName = n.lName,
                mobilePhone = n.mobilePhone,
                clinicPhone = n.clinicPhone,
                available =n.available,
                city = n.city,
                government = n.government,
                img =n.img,
                description =n.description,
                offer =n.offer,
                offerDate=n.offerDate,
                time =n.time,
                depts =n.depts,
                dept_id=n.dept_id
            }).OrderByDescending(n=>n.id).ToList();
            if (obj == null) return BadRequest();
             return Ok(obj);
        }
        //GetById/10
        //http://localhost:8276/Doctors/GetById/3
        [HttpGet]
        public ActionResult GetById(int id)
        {
            var doc = _context.Doctors.Include(n => n.depts).FirstOrDefault(n => n.id == id);
            if(doc == null) return BadRequest();

            return Ok(doc);
        }
        //GetByFirstname
        //http://localhost:8276/Doctors/GetByName?name=Ahmed
        [HttpGet]
        public ActionResult GetByName(string name)
        {
            var doc = _context.Doctors.Include(n => n.depts).Where(n => n.fName == name).ToList();
            if (doc == null) return BadRequest();

            return Ok(doc);
        }
        //GetByGovernment
        //http://localhost:8276/Doctors/GetByLocation?loc=Kafr%20El-Sheikh
        [HttpGet]
        public ActionResult GetByLocation(string loc)
        {
            var doc = _context.Doctors.Include(n => n.depts).Where(n => n.government == loc).ToList();
            if (doc == null) return BadRequest();

            return Ok(doc);
        }
        //getLatestOffer 
        [HttpGet]
        //http://localhost:8276/Doctors/LatestOffer
        public ActionResult LatestOffer()
        {
            var obj = _context.Doctors.Include(n => n.depts).Select(n => new Doctor()
            {
                id = n.id,
                fName = n.fName,
                lName = n.lName,
                mobilePhone = n.mobilePhone,
                clinicPhone = n.clinicPhone,
                available = n.available,
                city = n.city,
                government = n.government,
                img = n.img,
                description = n.description,
                offer = n.offer,
                offerDate = n.offerDate,
                time = n.time,
                depts = n.depts,
                dept_id = n.dept_id
            }).OrderByDescending(n => n.offerDate).ToList();
            if (obj == null) return BadRequest();
            return Ok(obj);
        }
        #endregion


        // GET: Doctors

        public async Task<IActionResult> Index()
        {
            var docContext = _context.Doctors.Include(d => d.depts).OrderBy(n=>n.fName).ThenBy(n=>n.lName);
            return View(await docContext.ToListAsync());
        }

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .Include(d => d.depts)
                .FirstOrDefaultAsync(m => m.id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // GET: Doctors/Create
        [Authorize(Roles = "Admin")]

        public IActionResult Create()
        {
            ViewData["dept_id"] = new SelectList(_context.Departments, "id", "name");
            return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DoctorViewModel doctor)
        {

            if (ModelState.IsValid)
            {

                var fullPath = Path.Combine(webhost.WebRootPath, "images", doctor.img.FileName);
                string imgext = Path.GetExtension(doctor.img.FileName);
                if (imgext == ".png" || imgext == ".jpg" || imgext ==".jpeg")
                {
                    using (var uploadimg1 = new FileStream(fullPath, FileMode.Create))
                    {
                        await doctor.img.CopyToAsync(uploadimg1);
                    }
                }
                Doctor newDoc = new Doctor
                {
                    fName = doctor.fName,
                    lName = doctor.lName,
                    mobilePhone = doctor.mobilePhone,
                    clinicPhone = doctor.clinicPhone,
                    img = doctor.img.FileName,
                    available = doctor.available,
                    description = doctor.description,
                    government = doctor.government,
                    city = doctor.city,
                    offer = doctor.offer,
                    offerDate= doctor.offerDate,
                    time = doctor.time,
                    dept_id = doctor.dept_id

                };
                _context.Add(newDoc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["dept_id"] = new SelectList(_context.Departments, "id", "name", doctor.dept_id);
            return View(doctor);
        }

        // GET: Doctors/Edit/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }
            ViewData["dept_id"] = new SelectList(_context.Departments, "id", "name", doctor.dept_id);
            return View(doctor);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,fName,lName,mobilePhone,clinicPhone,img,available,time,description,government,city,offer,offerDate,dept_id")] Doctor doctor)
        {
            if (id != doctor.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doctor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorExists(doctor.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["dept_id"] = new SelectList(_context.Departments, "id", "name", doctor.dept_id);
            return View(doctor);
        }

        // GET: Doctors/Delete/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .Include(d => d.depts)
                .FirstOrDefaultAsync(m => m.id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorExists(int id)
        {
            return _context.Doctors.Any(e => e.id == id);
        }
    }
}
