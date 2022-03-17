#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BEDAssignment2.Data;
using BEDAssignment2.Models;

namespace BEDAssignment2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : Controller
    {

        private readonly ModelDB _context;

        public ExpensesController(ModelDB context)
        {
            _context = context;
        }


        [HttpPost]
        public async Task<ActionResult<Expense>> OnPost(Expense expense)
        {

            var ExpenseinJobs = await _context.Jobs.FindAsync(expense.ExpenseId);
            if (ExpenseinJobs == null)
            {
                return BadRequest();
            }

            var ExpenseinModels = await _context.Models.FindAsync(expense.ExpenseId);
            if (ExpenseinModels == null)
            {
                return BadRequest();
            }

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            return expense;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expense>>> GetAllExpenses()
        {
            return await _context.Expenses.ToListAsync();
        }




        /*

        // GET: Expenses
        public async Task<IActionResult> Index()
        {
            return View(await _context.Expense.ToListAsync());
        }

        // GET: Expenses/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expense
                .FirstOrDefaultAsync(m => m.ExpenseId == id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        // GET: Expenses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Expenses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExpenseId,ModelId,JobId,Date,Text,amount")] Expense expense)
        {
            if (ModelState.IsValid)
            {
                _context.Add(expense);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(expense);
        }

        // GET: Expenses/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expense.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }
            return View(expense);
        }

        // POST: Expenses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ExpenseId,ModelId,JobId,Date,Text,amount")] Expense expense)
        {
            if (id != expense.ExpenseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expense);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenseExists(expense.ExpenseId))
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
            return View(expense);
        }

        // GET: Expenses/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expense
                .FirstOrDefaultAsync(m => m.ExpenseId == id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        // POST: Expenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var expense = await _context.Expense.FindAsync(id);
            _context.Expense.Remove(expense);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExpenseExists(long id)
        {
            return _context.Expense.Any(e => e.ExpenseId == id);
        }
        */
    }
}
