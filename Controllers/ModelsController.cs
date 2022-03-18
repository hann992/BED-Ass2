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

            // Mapping fra input model til retur model!
            ModelWithoutExpensesWithoutJobs returnModel = new ModelWithoutExpensesWithoutJobs(model.FirstName, model.LastName, model.Email, model.PhoneNo, model.AddresLine1, model.AddresLine2, model.Zip, model.City, model.BirthDay, model.Height, model.ShoeSize, model.HairColor, model.Comments);

            // Tilføj Model til context
            _context.Models.Add(newModel);

            // Gem data
            await _context.SaveChangesAsync();


            // Hvordan man tager fat i data på tværs af tabeller:
            /*     
            var modelB = await _context.Models.LastAsync();

            Console.WriteLine("Modelname: " + modelB.FirstName);
            foreach(var job in modelB.Jobs)
            {
                Console.WriteLine("BITCH: " + job.Customer);
                foreach(var modelss in job.Models)
                {
                    Console.WriteLine("Job Models: " + modelss.FirstName);
                }
            }
            */

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
                modelsList.Add(new ModelWithoutExpensesWithoutJobs(model.FirstName, model.LastName, model.Email, model.PhoneNo, model.AddresLine1, model.AddresLine2, model.Zip, model.City, model.BirthDay, model.Height, model.ShoeSize, model.HairColor, model.Comments));
            }

            return modelsList.ToList();
        }

        /// <summary>
        /// Fetch a Model w. jobs & expenses
        /// </summary>
        /// <param name="id">Model Id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Model>> GetModel(long? id)
        {
            var model = await _context.Models.FindAsync(id);
            /*List<Expense>e1 = */_context.Expenses.ToList();
            /*foreach(Expense expense in e1)
                Console.WriteLine(expense);*/
            //model.Expenses = e1.
            if (model == null)
            {
                return NotFound();
            }

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


        [HttpPut("{id}")]
        //public async Task<ActionResult<Model>> Edit(ModelWithoutExpensesWithoutJobs model)
        public async Task<ActionResult<Model>> Edit(long id, string firstName, string lastName, string email, string phoneNo, string addresLine1, string addresLine2, string zip, string city, DateTime birthDay, double height, int shoeSize, string hairColor, string comments)
        {
            //Find  modellen via ID'et
            var oldModel = await _context.Models.FindAsync(id);
            if (oldModel == null)
            {
                return NotFound();
            }

            if (firstName != null)
            {
                oldModel.FirstName = firstName;
            }

            if (lastName != null)
            {
                oldModel.LastName = lastName;
            }
            if (email != null)
            {
                oldModel.Email = email;
            }
            if (phoneNo != null)
            {
                oldModel.PhoneNo = phoneNo;
            }
            if (addresLine1 != null)
            {
                oldModel.AddresLine1 = addresLine1;
            }
            if (addresLine2 != null)
            {
                oldModel.AddresLine2 = addresLine2;
            }
            if (zip != null)
            {
                oldModel.Zip = zip;
            }
            if (city != null)
            {
                oldModel.City = city;
            }
            if (birthDay != null)
            {
                oldModel.BirthDay = birthDay;
            }
            if (height != null)
            {
                oldModel.Height = height;
            }
            if (shoeSize != null)
            {
                oldModel.ShoeSize = shoeSize;
            }
            if (hairColor != null)
            {
                oldModel.HairColor = hairColor;
            }
            if (comments != null)
            {
                oldModel.Comments = comments;
            }


            //Model model1 = new Model(firstName,  lastName,  email,  phoneNo,  addresLine1,  addresLine2,  zip,  city,  birthDay,  height,  shoeSize,  hairColor, comments);
            //model1.ModelId = id;
            //model1.Expenses = oldModel.Expenses;
            //model1.Jobs = oldModel.Jobs;

            /*
            var oldModel = await _context.Models.FindAsync(model.ModelId);

            if (oldModel == null)
            {
                return NotFound();
            }

            if (model.FirstName != null)
            {
                oldModel.FirstName = model.FirstName;
            }
            if (model.LastName != null)
            {
                oldModel.LastName = model.LastName;
            }
            if (model.Email != null)
            {
                oldModel.Email = model.Email;
            }
            if (model.PhoneNo != null)
            {
                oldModel.PhoneNo = model.PhoneNo;
            }
            if (model.AddresLine1 != null)
            {
                oldModel.AddresLine1 = model.AddresLine1;
            }
            if (model.AddresLine2 != null)
            {
                oldModel.AddresLine2 = model.AddresLine2;
            }
            if (model.City != null)
            {
                oldModel.City = model.City;
            }
            if (model.Zip != null)
            {
                oldModel.Zip = model.Zip;
            }
            if (model.BirthDay != null)
            {
                oldModel.BirthDay = model.BirthDay;
            }
            if (model.Height != null)
            {
                oldModel.Height = model.Height;
            }
            if (model.ShoeSize != null)
            {
                oldModel.ShoeSize = model.ShoeSize;
            }
            if (model.HairColor != null)
            {
                oldModel.HairColor = model.HairColor;
            }
            if (model.Comments != null)
            {
                oldModel.Comments = model.Comments;
            }
            */
            // Gem ændringer
            await _context.SaveChangesAsync();

            // Returner den redigerede
            return oldModel;
        }


        /*
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

            if (oldModel.FirstName != null){oldModel.FirstName = model.FirstName;}
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
        */


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
