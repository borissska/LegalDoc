#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LegalDoc.Models;
using SolrNet;
using SolrNet.Impl;
using CommonServiceLocator;
using LegalDoc.CRUDSolr;

namespace LegalDoc.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly ApplicationContext _context;

        public DocumentsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Documents
        public async Task<IActionResult> Index()
        {
            return View(await _context.documents.ToListAsync());
        }

        // GET: Documents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.documents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // GET: Documents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,DocText,AcceptanceTime,Author")] Document document)
        {
            Operations operations = new Operations();
            if (ModelState.IsValid)
            {
                _context.Add(document);
                await _context.SaveChangesAsync();

                operations.Create(document);

                return LocalRedirectPermanent("~/Documents/Edit/{Id?}");
            }
            return View(document);
        }

        // GET: Documents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.documents.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }
            return View(document);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,DocText,AcceptanceTime,Author")] Document document)
        {
            Operations operations = new Operations();
            if (id != document.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    operations.Update(id, document);
                    _context.Update(document);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentExists(document.Id))
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
            return View(document);
        }

        // GET: Documents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.documents
                .FirstOrDefaultAsync(m => m.Id == id);

            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Operations operations = new Operations(); 
            var document = await _context.documents.FindAsync(id);
            operations.Delete(id);
            _context.documents.Remove(document);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Documents/Find/stroka
        [HttpPost, ActionName("Find")]
        public IActionResult Find([FromForm]SendingObj sendingObj)
        {
            Operations operations = new Operations();
            string stroka = operations.ClearSentence(sendingObj.Stroka.ToString());

            if (stroka.Length == 0)
                return NotFound();

            List<int> documentIds = operations.Find(stroka);

            if (documentIds == null)
            {
                return LocalRedirectPermanent("~/Documents/Create");
            }

            List<Document> documentsList = new List<Document>();

            foreach (Document document in _context.documents)
                foreach (int documentId in documentIds) 
                    if (document.Id == documentId)
                        documentsList.Add(document);

            sendingObj.Document = documentsList;

            return View(sendingObj);
        }



        private bool DocumentExists(int id)
        {
            return _context.documents.Any(e => e.Id == id);
        }
    }
}
