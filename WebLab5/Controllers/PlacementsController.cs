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
    public class PlacementsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlacementsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Placements
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

            this.ViewBag.Ward = ward;

            var placements = await this._context.Placements
                .Include(w => w.Ward)
                .Where(x => x.WardId == WardId)
                .ToListAsync();

            return this.View(placements);

        }

        // GET: Placements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var placement = await _context.Placements
                .Include(p => p.Ward)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (placement == null)
            {
                return NotFound();
            }

            return View(placement);
        }

        // GET: Placements/Create
        public async Task<IActionResult> Create(Int32? WardId)
        {
            if (WardId == null)
            {
                return NotFound();
            };
            var ward = await _context.Wards
                .SingleOrDefaultAsync(m => m.Id == WardId);

            if (ward == null)
            {
                return NotFound();
            };

            ViewBag.Ward = ward;
            return this.View(new PlacementViewModel());
        }

        // POST: Placements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? WardId ,PlacementViewModel model)
        {

            if (WardId == null)
            {
                return NotFound();
            };
            var ward = await _context.Wards
                .SingleOrDefaultAsync(m => m.Id == WardId);

            if (ward == null)
            {
                return NotFound();
            };

            if (ModelState.IsValid)
            {

                var placement = new Placement
                {
                    Bed = model.Bed,
                    WardId = ward.Id
                };

                _context.Add(placement);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { wardId = ward.Id });
            }

            this.ViewBag.Ward = ward;
            return View(model);
        }

        // GET: Placements/Edit/5
        public async Task<IActionResult> Edit(Int32? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var placement = await _context.Placements.SingleOrDefaultAsync(m => m.Id == id);
            if (placement == null)
            {
                return NotFound();
            }

            var model = new PlacementViewModel
            {
                Bed = placement.Bed
            };

            return View(model); 
        }

        // POST: Placements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? id, PlacementViewModel model)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var placement = await this._context.Placements
               .SingleOrDefaultAsync(m => m.Id == id);
            if (placement == null)
            {
                return this.NotFound();
            }

            if (ModelState.IsValid)
            {
                placement.Bed = model.Bed;
                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { wardId = placement.WardId });

            }
            return View(model);
        }

        // GET: Placements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var placement = await _context.Placements
                .Include(p => p.Ward)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (placement == null)
            {
                return NotFound();
            }

            return View(placement);
        }

        // POST: Placements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var placement = await _context.Placements.SingleOrDefaultAsync(m => m.Id == id);
            _context.Placements.Remove(placement);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { wardId = placement.WardId });
        }

        private bool PlacementExists(int id)
        {
            return _context.Placements.Any(e => e.Id == id);
        }
    }
}
