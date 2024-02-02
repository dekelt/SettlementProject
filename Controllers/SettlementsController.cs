using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SettlementProject.Data;
using SettlementProject.Models;

namespace SettlementProject.Controllers
{
    public class SettlementsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SettlementsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Settlements
        public async Task<IActionResult>Index(string sortSettlements)
        {
                return View(await GetSettlementsList(1, sortSettlements));
        }
        [HttpPost]
        public async Task<IActionResult>Index(int CurrentPageIndex, string sortSettlements, string SearchText="")
        {
            return View(await GetSettlementsList(CurrentPageIndex, sortSettlements, SearchText));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var settlement = await _context.Settlement
                .FirstOrDefaultAsync(m => m.Id == id);
            if (settlement == null)
            {
                return NotFound();
            }

            return View(settlement);
        }

        // GET: Settlements/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Settlements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SettlementName")] Settlement settlement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(settlement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(settlement);
        }

        // GET: Settlements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var settlement = await _context.Settlement.FindAsync(id);
            if (settlement == null)
            {
                return NotFound();
            }
            return View(settlement);
        }

        // POST: Settlements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SettlementName")] Settlement settlement)
        {
            if (id != settlement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(settlement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SettlementExists(settlement.Id))
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
            return View(settlement);
        }

        // GET: Settlements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var settlement = await _context.Settlement
                .FirstOrDefaultAsync(m => m.Id == id);
            if (settlement == null)
            {
                return NotFound();
            }

            return View(settlement);
        }

        // POST: Settlements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var settlement = await _context.Settlement.FindAsync(id);
            _context.Settlement.Remove(settlement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SettlementExists(int id)
        {
            return _context.Settlement.Any(e => e.Id == id);
        }

        private async Task<SettlementsView> GetSettlementsList(int CurrentPageIndex, string sortSettlements, string SearchText = "")
        {
            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortSettlements) ? "name_desc" : "";

            int maxRowPerPage = 5;
            SettlementsView settlementsView = new SettlementsView();
            if (SearchText != "" && SearchText != null)//בדיקת חיפוש
            {
                if (sortSettlements == "name_desc")//מיון סדר עולה
                {
                    settlementsView.SettlementList = await (from settlemen in _context.Settlement where (settlemen.SettlementName.Contains(SearchText)) select settlemen)//חיפוש+סדר עולה
                        .OrderBy(x => x.SettlementName)
                        .Skip((CurrentPageIndex - 1) * maxRowPerPage)
                        .Take(maxRowPerPage)
                        .ToListAsync();
                }
                else
                    settlementsView.SettlementList = await (from settlemen in _context.Settlement where (settlemen.SettlementName.Contains(SearchText)) select settlemen)//חיפוש + סדר יורד
                       .OrderByDescending(x => x.SettlementName)
                     //  .Skip((CurrentPageIndex - 1) * maxRowPerPage)
                       .Take(maxRowPerPage)
                       .ToListAsync();
            }
            else
               if (sortSettlements == null)
            {
                settlementsView.SettlementList = await (from settlemen in _context.Settlement select settlemen)//אין חיפוש סדר יורד
                    .OrderByDescending(x => x.SettlementName)
                    .Skip((CurrentPageIndex - 1) * maxRowPerPage)
                    .Take(maxRowPerPage)
                    .ToListAsync();
            }
            else
            {
                settlementsView.SettlementList = await (from settlemen in _context.Settlement select settlemen)//אין חיפוש סדר עולה
                   .OrderBy(x => x.SettlementName)
                   .Skip((CurrentPageIndex - 1) * maxRowPerPage)
                   .Take(maxRowPerPage)
                   .ToListAsync();
            }

            double pageCount = (double)((decimal)_context.Settlement.Count() / Convert.ToDecimal(maxRowPerPage));
            settlementsView.PageCount = (int)Math.Ceiling(pageCount);
            settlementsView.CurrentPageIndex = CurrentPageIndex;
            return settlementsView;
        }

    }
}


