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

            // Tilføj Model til context
            _context.Models.Add(newModel);

            // Gem data
            await _context.SaveChangesAsync();

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

            //laver en liste af models uden udgifter og jobs
            List<ModelWithoutExpensesWithoutJobs> modelsList = new List<ModelWithoutExpensesWithoutJobs>();

            // for hver model i modelsDB overføres til vores modelslist af modeller uden udgifter og jobs. 
            foreach(var model in models)
            {
                modelsList.Add(new ModelWithoutExpensesWithoutJobs(model.ModelId, model.FirstName, model.LastName, model.Email, model.PhoneNo, model.AddresLine1, model.AddresLine2, model.Zip, model.City, model.BirthDay, model.Height, model.ShoeSize, model.HairColor, model.Comments));
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
            //tjek om modellen findes
            var model = await _context.Models.FindAsync(id);

            //caster vores expenses til en liste
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
            // Gem modellen
            await _context.SaveChangesAsync();

            // Returner den slettede model
            return model;
        }


        // Opdatere en model:
        [HttpPut("{id}")]
        public async Task<ActionResult<Model>> Edit(long id, string firstName, string lastName, string email, string phoneNo, string addresLine1, string addresLine2, string zip, string city, DateTime birthDay, double height, int shoeSize, string hairColor, string comments)
        {
            //Find  modellen via ID
            var oldModel = await _context.Models.FindAsync(id);
            if (oldModel == null)
            {
                return NotFound();
            }
            //tjek om de forskellige værdier at andet en null
            // for derefter at sætte dem til oldmodel's værdier.
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

            // Gem ændringerne
            await _context.SaveChangesAsync();

            // Returner den redigerede
            return oldModel;
        }

    }
}
