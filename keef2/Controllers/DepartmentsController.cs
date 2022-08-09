using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Keefa1.Models;
using Microsoft.AspNetCore.Hosting;
using keef2.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace keef2.Controllers
{
    [EnableCors("mypolicy")]

    public class DepartmentsController : Controller
    {
        private readonly DocContext _context;
        private readonly IWebHostEnvironment webhost;

        public DepartmentsController(DocContext context, IWebHostEnvironment _webhost)
        {
            _context = context;
            webhost = _webhost;
        }

        #region APIs
        //GetAll
        //Departments/GetAll
        [HttpGet]
        public ActionResult GetAll()
        {
            var obj = _context.Departments.ToList();
            if (obj == null) return NotFound();
            return Ok(obj);
        }

        //GetById
        //Departments/GetById/3
        public ActionResult GetById(int id )
        {
            var obj = _context.Departments.FirstOrDefault(n => n.id == id);
            if (obj == null) return NotFound();
            return Ok(obj);
        }

        //GetByName
        //Departments/GetByName?name=Cardiology
        public ActionResult GetByName(string name)
        {
            var obj = _context.Departments.FirstOrDefault(n => n.name == name);
            if (obj == null) return NotFound();
            return Ok(obj);
        }
        #endregion


        // GET: Departments
        public async Task<IActionResult> Index()
        {
            return View(await _context.Departments.ToListAsync());
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var doctor = _context.Doctors.Where(n => n.dept_id == id).ToList();
            ViewData["docs"] = doctor;

            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .FirstOrDefaultAsync(m => m.id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }
       
        
        
        [Authorize(Roles ="Admin")]
        // GET: Departments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,img")] DepartmentViewModel department)
        {
            if (ModelState.IsValid)
            {
                string fileName = "";
                if(department.img != null)
                {
                fileName = department.img.FileName;
                var fullPath = Path.Combine(webhost.WebRootPath, "images", fileName);
                string imgext = Path.GetExtension(department.img.FileName);
                if (imgext == ".png" || imgext == ".jpg")
                {
                    using (var uploadimg1 = new FileStream(fullPath, FileMode.Create))
                    {
                        await department.img.CopyToAsync(uploadimg1);
                    }
                }   

                }

                Department dept = new Department
                {
                    name = department.name,
                    img = fileName
                };

                _context.Add(dept);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }


        [Authorize(Roles = "Admin")]
        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        // POST: Departments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,img")] Department department)
        {
            if (id != department.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.id))
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
            return View(department);
        }


        [Authorize(Roles = "Admin")]
        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .FirstOrDefaultAsync(m => m.id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        [Authorize(Roles = "Admin")]
        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.id == id);
        }
    }
}
