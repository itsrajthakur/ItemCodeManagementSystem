using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Identity.Data;
using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Identity.ViewModels;

namespace Identity.Controllers
{
    [Authorize]
    public class MappingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MappingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // product ItemCode
        [NonAction]
        private void LoadProducts()
        {
            var products = _context.Products.ToList();
            ViewBag.Products = new SelectList(products, "Id", "ItemCode");
        }

        // GET: Mapping
        public async Task<IActionResult> Index()
        {
            var mappings = await _context.Mappings
                .Select(m => new MappingViewModel
                {
                    Id = m.Id,
                    ProductId = m.ProductId,
                    UserPrice = m.UserPrice,
                    UserItemCode = m.UserItemCode,
                    ItemCode = _context.Products
                                        .Where(p => p.Id == m.ProductId) // ProductId should match p.Id type
                                        .Select(p => p.ItemCode)
                                        .FirstOrDefault()
                })
                .ToListAsync();

            return View(mappings);
        }

        // GET: Mapping/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mapping = await _context.Mappings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mapping == null)
            {
                return NotFound();
            }

            return View(mapping);
        }

        // GET: Mapping/Create
        public IActionResult Create()
        {
            LoadProducts();
            return View();
        }

        // POST: Mapping/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,UserPrice,UserItemCode")] Mapping mapping)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mapping);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            LoadProducts();
            return View(mapping);
        }

        // GET: Mapping/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            LoadProducts();
            var mapping = await _context.Mappings.FindAsync(id);
            if (mapping == null)
            {
                return NotFound();
            }
            return View(mapping);
        }

        // POST: Mapping/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,UserPrice,UserItemCode")] Mapping mapping)
        {
            if (id != mapping.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mapping);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MappingExists(mapping.Id))
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
            return View(mapping);
        }

        // GET: Mapping/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mapping = await _context.Mappings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mapping == null)
            {
                return NotFound();
            }

            return View(mapping);
        }

        // POST: Mapping/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mapping = await _context.Mappings.FindAsync(id);
            if (mapping != null)
            {
                _context.Mappings.Remove(mapping);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MappingExists(int id)
        {
            return _context.Mappings.Any(e => e.Id == id);
        }
    }
}
