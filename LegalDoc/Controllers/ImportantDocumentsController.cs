#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LegalDoc.Models;

namespace LegalDoc.Controllers
{
    public class ImportantDocumentsController : Controller
    {
        private readonly ApplicationContext _context;

        public ImportantDocumentsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: ImportantDocuments
        public async Task<IActionResult> Index()
        {
            var applicationContext = _context.importantDocuments.Include(i => i.Document).Include(i => i.User);
            return View(await applicationContext.ToListAsync());
        }

        // GET: ImportantDocuments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var importantDocuments = await _context.importantDocuments
                .Include(i => i.Document)
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (importantDocuments == null)
            {
                return NotFound();
            }

            return View(importantDocuments);
        }

        // GET: ImportantDocuments/Create
        public IActionResult Create()
        {
            ViewData["DocumentId"] = new SelectList(_context.documents, "Id", "Id");
            ViewData["UserID"] = new SelectList(_context.users, "Id", "Id");
            return View();
        }

        // POST: ImportantDocuments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserID,DocumentId")] ImportantDocuments importantDocuments)
        {
            if (ModelState.IsValid)
            {
                _context.Add(importantDocuments);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DocumentId"] = new SelectList(_context.documents, "Id", "Id", importantDocuments.DocumentId);
            ViewData["UserID"] = new SelectList(_context.users, "Id", "Id", importantDocuments.UserID);
            return View(importantDocuments);
        }

        // GET: ImportantDocuments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var importantDocuments = await _context.importantDocuments.FindAsync(id);
            if (importantDocuments == null)
            {
                return NotFound();
            }
            ViewData["DocumentId"] = new SelectList(_context.documents, "Id", "Id", importantDocuments.DocumentId);
            ViewData["UserID"] = new SelectList(_context.users, "Id", "Id", importantDocuments.UserID);
            return View(importantDocuments);
        }

        // POST: ImportantDocuments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserID,DocumentId")] ImportantDocuments importantDocuments)
        {
            if (id != importantDocuments.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(importantDocuments);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImportantDocumentsExists(importantDocuments.Id))
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
            ViewData["DocumentId"] = new SelectList(_context.documents, "Id", "Id", importantDocuments.DocumentId);
            ViewData["UserID"] = new SelectList(_context.users, "Id", "Id", importantDocuments.UserID);
            return View(importantDocuments);
        }

        // GET: ImportantDocuments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var importantDocuments = await _context.importantDocuments
                .Include(i => i.Document)
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (importantDocuments == null)
            {
                return NotFound();
            }

            return View(importantDocuments);
        }

        // POST: ImportantDocuments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var importantDocuments = await _context.importantDocuments.FindAsync(id);
            _context.importantDocuments.Remove(importantDocuments);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImportantDocumentsExists(int id)
        {
            return _context.importantDocuments.Any(e => e.Id == id);
        }
    }
}
