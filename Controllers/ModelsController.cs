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
using System.Globalization;

namespace BEDAssignment2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [BindProperties(SupportsGet = true)]
    public class ModelsController : Controller
    {
        private readonly ModelDB _context;

        public ModelsController(ModelDB context)
        {
            _context = context;
        }

        /// <summary>
        /// Add a new Model
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ModelWithoutExpensesWithoutJobs>> OnPost(ModelWithoutIdWithoutExpensesWithoutJobs model)
                                                        
        {
            // Mapping fra input model til endelig model!
            Model newModel = new Model(model.FirstName, model.LastName, model.Email, model.PhoneNo, model.AddresLine1, model.AddresLine2, model.Zip, model.City, model.BirthDay, model.Height, model.ShoeSize, model.HairColor, model.Comments);

            Job newJob = new Job("Dick", DateTimeOffset.Now, 3, "dick", "dick");

            newModel.Jobs.Add(newJob);

            Job newJo2b = new Job("Dic2k", DateTimeOffset.Now, 3, "d2ick", "di2ck");

            newModel.Jobs.Add(newJo2b);

            Console.WriteLine("Jobs: "+newModel.Jobs.Count);


            

            // Tilføj Model til context
            _context.Models.Add(newModel);

            

            // Gem data
            _context.SaveChanges();

            Console.WriteLine("No. Jobs: " + _context.Models.LastAsync().Result.Jobs.Count());

            // Mapping fra input model til retur model!
            ModelWithoutExpensesWithoutJobs returnModel = new ModelWithoutExpensesWithoutJobs(_context.Models.Last().ModelId, model.FirstName, model.LastName, model.Email, model.PhoneNo, model.AddresLine1, model.AddresLine2, model.Zip, model.City, model.BirthDay, model.Height, model.ShoeSize, model.HairColor, model.Comments);






            // Returner seneste tilføjede model
            return returnModel;

            

        }


        // GET alle Models
        /// <summary>
        /// Fetch all Models
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModelWithoutExpensesWithoutJobs>>> GetModelsWithInfo()
        {
            List<Model> models = await _context.Models.ToListAsync();

            List<ModelWithoutExpensesWithoutJobs> modelsList = new List<ModelWithoutExpensesWithoutJobs>();

            foreach(var model in models)
            {
                modelsList.Add(new ModelWithoutExpensesWithoutJobs(model.ModelId, model.FirstName, model.LastName, model.Email, model.PhoneNo, model.AddresLine1, model.AddresLine2, model.Zip, model.City, model.BirthDay, model.Height, model.ShoeSize, model.HairColor, model.Comments));
            }


            return modelsList.ToList();
        }



        /// <summary>
        /// Fetch a Model with jobs and expeses
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
            Console.WriteLine("No. Jobs: " + _context.Models.LastAsync().Result.Jobs.Count());

            Console.WriteLine("no2 jobs: " + _context.Jobs.Count());

            foreach (Job job in _context.Jobs)
            {
                Console.WriteLine("Jobname: " + job.Customer);
                foreach (Model model2 in job.Models)
                {
                    Console.WriteLine(model2.FirstName);
                }
                
            }

            model.Jobs = _context.Jobs.ToList();

            return model;
        }


        // Slet en model:

        /// <summary>
        /// Delete a Model by Id
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

            // Slet modellen
            _context.Models.Remove(model);

            // Gem
            await _context.SaveChangesAsync();

            // Returner den slettede model
            return model;
        }

        // Rediger en model:

        /// <summary>
        /// Edit a Models details
        /// </summary>
        /// <param name="FirstName">Model first Name</param>
        /// <param name="LastName">Model last name</param>
        /// <param name="Email">Email</param>
        /// <param name="PhoneNo">Phone No.</param>
        /// <param name="AddresLine1">Addresline 1</param>
        [HttpPut("{id}")]
        public async Task<ActionResult<Model>> Edit(long? id, [Bind("FirstName,LastName,Email,PhoneNo,AddresLine1,AddresLine2,Zip,City,BirthDay,Height,ShoeSize,HairColor,Comments")] Model model)
        {
            //er id null, så skal der intet gøres.
            if (id == null)
            {
                return NotFound();
            }

            //Find  modellen via ID'et
            var oldModel = await _context.Models.FindAsync(id);
            if (oldModel == null)
            {
                return NotFound();
            }

            if (oldModel.FirstName != null) oldModel.FirstName = model.FirstName;
            if (oldModel.LastName != null) oldModel.LastName = model.LastName;
            if (oldModel.Email != null) oldModel.Email = model.Email;
            if (oldModel.PhoneNo != null) oldModel.PhoneNo = model.PhoneNo;
            if (oldModel.AddresLine1 != null) oldModel.AddresLine1 = model.AddresLine1;
            if (oldModel.AddresLine2 != null) oldModel.AddresLine2 = model.AddresLine2;
            if (oldModel.Zip != null) oldModel.Zip = model.Zip;
            if (oldModel.City != null) oldModel.City = model.City;
            if (oldModel.BirthDay != null) oldModel.BirthDay = model.BirthDay;
            if (oldModel.Height != null) oldModel.Height = model.Height;
            if (oldModel.ShoeSize != null) oldModel.ShoeSize = model.ShoeSize;
            if (oldModel.HairColor != null) oldModel.HairColor = model.HairColor;
            if (oldModel.Comments != null) oldModel.Comments = model.Comments;



            // Gem ændringer
            await _context.SaveChangesAsync();

            // Returner den redigerede
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
