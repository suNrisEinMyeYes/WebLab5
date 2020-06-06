using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebLab5.Data;
using WebLab5.Models;
using WebLab5.Models.ViewModels;

namespace WebLab5.Controllers
{
    public class WardStaffsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WardStaffsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WardStaffs
        public async Task<IActionResult> Index(Int32? WardId)
        {
            if (WardId == null)
            {
                return this.NotFound();
            }

            var ward = await this._context.Wards
                .SingleOrDefaultAsync(x => x.Id == WardId);

            if (ward == null)
            {
                return this.NotFound();
            }

            ViewBag.Ward = ward;

            var wardstaff = await this._context.WardStaff
                .Where(x => x.WardId == WardId)
                .ToListAsync();

            return View(wardstaff);
        }

        // GET: WardStaffs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wardStaff = await _context.WardStaff
                .Include(w => w.Ward)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (wardStaff == null)
            {
                return NotFound();
            }

            return View(wardStaff);
        }

        // GET: WardStaffs/Create
        public async Task<IActionResult> Create(Int32? WardId)
        {
            if (WardId == null)
            {
                return this.NotFound();
            }

            var ward = await this._context.Wards
                .SingleOrDefaultAsync(x => x.Id == WardId);

            if (ward == null)
            {
                return this.NotFound();
            }

            ViewBag.Ward = ward;
            return View(new WardStaffViewModel());
        }

        // POST: WardStaffs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? WardId, WardStaffViewModel model)
        {
            if (WardId == null)
            {
                return this.NotFound();
            }

            var ward = await this._context.Wards
                .SingleOrDefaultAsync(x => x.Id == WardId);

            if (ward == null)
            {
                return this.NotFound();
            }


            if (ModelState.IsValid)
            {
                var WS = new WardStaff
                {
                    Name = model.Name,
                    Position = model.Position,
                    WardId = ward.Id
                };

                _context.Add(WS);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { wardId = ward.Id });
            }
            this.ViewBag.Ward = ward;
            return this.View(model);

        }

        // GET: WardStaffs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wardStaff = await _context.WardStaff.SingleOrDefaultAsync(m => m.Id == id);
            if (wardStaff == null)
            {
                return NotFound();
            }
            ViewData["WardId"] = new SelectList(_context.Wards, "Id", "Name", wardStaff.WardId);
            return View(wardStaff);
        }

        // POST: WardStaffs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Position,WardId")] WardStaff wardStaff)
        {
            if (id != wardStaff.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wardStaff);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WardStaffExists(wardStaff.Id))
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
            ViewData["WardId"] = new SelectList(_context.Wards, "Id", "Name", wardStaff.WardId);
            return View(wardStaff);
        }

        // GET: WardStaffs/Delete/5
        public async Task<IActionResult> Delete(Int32? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wardStaff = await _context.WardStaff
                .Include(w => w.Ward)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (wardStaff == null)
            {
                return NotFound();
            }

            return View(wardStaff);
        }

        // POST: WardStaffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32 id)
        {
            var wardStaff = await _context.WardStaff.SingleOrDefaultAsync(m => m.Id == id);
            _context.WardStaff.Remove(wardStaff);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { wardId = wardStaff.WardId });
        }

        private bool WardStaffExists(Int32 id)
        {
            return _context.WardStaff.Any(e => e.Id == id);
        }
    }
}
