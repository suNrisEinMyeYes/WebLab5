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
    public class HospitalLabsController : Controller
    {
        private readonly ApplicationDbContext context;

        public HospitalLabsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: HospitalLabs
        public async Task<IActionResult> Index(Int32? hospitalId)
        {
            if (hospitalId == null)
            {
                return this.NotFound();
            }

            var hospital = await this.context.Hospitals
                .SingleOrDefaultAsync(x => x.Id == hospitalId);

            if (hospital == null)
            {
                return this.NotFound();
            }

            var items = await this.context.HospitalLabs
                .Include(h => h.Hospital)
                .Include(h => h.Lab)
                .Where(x => x.HospitalId == hospital.Id)
                .ToListAsync();
            this.ViewBag.Hospital = hospital;
            return this.View(items);
        }

        // GET: HospitalLabs/Create
        public async Task<IActionResult> Create(Int32? hospitalId)
        {
            if (hospitalId == null)
            {
                return this.NotFound();
            }

            var hospital = await this.context.Hospitals
                .SingleOrDefaultAsync(x => x.Id == hospitalId);

            if (hospital == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Hospital = hospital;
            this.ViewData["LabId"] = new SelectList(this.context.Labs, "Id", "Name");
            return this.View(new HospitalLabCreateModel());
        }

        // POST: HospitalLabs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? hospitalId, HospitalLabCreateModel model)
        {
            if (hospitalId == null)
            {
                return this.NotFound();
            }

            var hospital = await this.context.Hospitals
                .SingleOrDefaultAsync(x => x.Id == hospitalId);

            if (hospital == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                var hospitalLab = new HospitalLab
                {
                    HospitalId = hospital.Id,
                    LabId = model.LabId
                };

                this.context.Add(hospitalLab);
                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { hospitalId = hospital.Id });
            }

            this.ViewBag.Hospital = hospital;
            this.ViewData["LabId"] = new SelectList(this.context.Labs, "Id", "Name", model.LabId);
            return this.View(model);
        }

        // GET: HospitalLabs/Delete/5
        public async Task<IActionResult> Delete(Int32? hospitalId, Int32? labId)
        {
            if (hospitalId == null || labId == null)
            {
                return this.NotFound();
            }

            var hospitalLab = await this.context.HospitalLabs
                .Include(h => h.Hospital)
                .Include(h => h.Lab)
                .SingleOrDefaultAsync(m => m.HospitalId == hospitalId && m.LabId == labId);
            if (hospitalLab == null)
            {
                return this.NotFound();
            }

            return this.View(hospitalLab);
        }

        // POST: HospitalLabs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32 hospitalId, Int32 labId)
        {
            var hospitalLab = await this.context.HospitalLabs.SingleOrDefaultAsync(m => m.HospitalId == hospitalId && m.LabId == labId);
            this.context.HospitalLabs.Remove(hospitalLab);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction("Index", new { hospitalId = hospitalId });
        }
    }
}
