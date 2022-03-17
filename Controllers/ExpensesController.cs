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
        public async Task<ActionResult<Expense>> OnPost(Expense expense)
        {
            var model = await _context.Models.FindAsync(expense.ModelId);
            if (model == null)
            {
                return BadRequest();
            }

            var job = await _context.Jobs.FindAsync(expense.JobId);
            if (job == null)
            {
                return BadRequest();
            }

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            model.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            job.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            return expense;
        }


        //mere til test :D
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expense>>> GetAllExpenses()
        {
            return await _context.Expenses.ToListAsync();
        }

        
    }
}
