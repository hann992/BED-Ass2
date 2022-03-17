#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BEDAssignment2.Data;
using BEDAssignment2.Models;

using Microsoft.AspNetCore.SignalR;
using BEDAssignment2.Hubs;

namespace BEDAssignment2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [BindProperties(SupportsGet = true)]
    public class ModelsController : Controller
    {
        private readonly ModelDB _context;
        private readonly IHubContext<ExpenseHub> _expenseHubContext;

        public ModelsController(ModelDB context, IHubContext<ExpenseHub> expenseHubContext)
        {
            _context = context;
            _expenseHubContext = expenseHubContext;
        }

        /// <summary>
        /// Add a new Model
        /// </summary>
        /// <param name="FirstName">Model firstname</param>
        /// /// <param name="LastName">Model lastname</param>
        [HttpPost("{FirstName}/{LastName}")]
        public async Task<ActionResult<Model>> OnPost(string FirstName, string LastName)
        {
            //dette virker helt fint dog med 
            _context.Models.Add(new Model(FirstName,LastName));
            await _context.SaveChangesAsync();
            await _expenseHubContext.Clients.All.SendAsync("ReceiveMessage");

            return _context.Models.Last();
        }

        
        // Expense
        // Model Id
        // Job Id

        // Expense(int ModelId,......)





        // GET metode for alle Models
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Model>>> GetModels()
        {
            return await _context.Models.ToListAsync();
        }

        /// <summary>
        /// Fetch a Model
        /// </summary>
        /// <param name="id">Model Id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Model>> GetModel(long? id)
        {
            var model = await _context.Models.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }


            //string jsonString = JsonSerializer.Serialize(weatherForecast);



            return model;
        }


        /// <summary>
        /// Delete a Model
        /// </summary>
        /// <param name="id">Model Id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Model>> Delete(long? id)
        {
            //er id null, så skal der intet gøres.
            if (id == null)
            {
                return NotFound();
            }

            //Find  modellen via ID'et
            var model = await _context.Models.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            _context.Models.Remove(model);
            await _context.SaveChangesAsync();
            //da den stadig er i ram, så vil jeg gerne se hvilken 
            //som er slettet.
            return model;
        }
        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ModelId,FirstName,LastName,Email,PhoneNo,AddresLine1,AddresLine2,Zip,City,BirthDay,Height,ShoeSize,HairColor,Comments")] Model model)
        {
            if (id != model.ModelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModelExists(model.ModelId))
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
            return View(model);
        }
        */
        /*

        [HttpPut("{id}")]
        public async Task<ActionResult<Model>> PutTodo(long id, Model model)
        {
            if (id != model.ModelId)
            {
                return BadRequest();
            }



            _context.Entry(Model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }

            catch (DbUpdateConcurrencyException)
            {
                if (!TodoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        */

        /*
        // GET: Models/Details/5

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.Models
                .FirstOrDefaultAsync(m => m.ModelId == id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: Models/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Models/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ModelId,FirstName,LastName,Email,PhoneNo,AddresLine1,AddresLine2,Zip,City,BirthDay,Height,ShoeSize,HairColor,Comments")] Model model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Models/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.Models.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Models/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ModelId,FirstName,LastName,Email,PhoneNo,AddresLine1,AddresLine2,Zip,City,BirthDay,Height,ShoeSize,HairColor,Comments")] Model model)
        {
            if (id != model.ModelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModelExists(model.ModelId))
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
            return View(model);
        }

        // GET: Models/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.Models
                .FirstOrDefaultAsync(m => m.ModelId == id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: Models/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var model = await _context.Models.FindAsync(id);
            _context.Models.Remove(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModelExists(long id)
        {
            return _context.Models.Any(e => e.ModelId == id);
        }
        */
    }
}
