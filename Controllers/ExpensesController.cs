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
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

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
        public async Task<ActionResult<Expense>> OnPost(ExpenseNoId expense)
        {
            // Kan vi finde modellen?
            var model = await _context.Models.FindAsync(expense.ModelId);

            // El er den null?
            if (model == null)
            {
                // så send BAD REQUEST:
                return BadRequest();
            }

            // Vi fandt modellen, findes jobbet?

            var job = await _context.Jobs.FindAsync(expense.JobId);

            // El er den null?
            if (job == null)
            {
                // så send BAD REQUEST:
                return BadRequest();
            }

            // Vi gemmer den nye expense
            _context.Expenses.Add(new Expense(expense));
            await _context.SaveChangesAsync();

            // Vi gemmer den nye expense i modellen også!
            model.Expenses.Add(_context.Expenses.Last());
            await _context.SaveChangesAsync();

            // Udelukkende til responsen:
            Expense newExpense = new Expense(expense);
            newExpense.ExpenseId = _context.Expenses.Last().ExpenseId;

            return newExpense;
        }

    }
}
