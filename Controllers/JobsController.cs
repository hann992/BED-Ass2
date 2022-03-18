using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BEDAssignment2.Data;
using BEDAssignment2.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Controller;

namespace BEDAssignment2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Jobscontroller : Controller
    {
        private readonly ModelDB _context;

        public Jobscontroller(ModelDB context)
        {
            _context = context;

        }

        [HttpPost] // opret nyt job
        public async Task<ActionResult<JobsWithoutExpensesWithoutModels>> OnPost(JobsWithoutIDWithoutExpensesWithoutModels job)
        {
            // Mapping fra input model til endelig model!
            Job newJob = new Job(job.Customer, job.StartDate,job.Days, job.Location, job.Comments);

            // Mapping fra input model til retur model!
            JobsWithoutExpensesWithoutModels returnModel = new JobsWithoutExpensesWithoutModels(job.Customer, job.StartDate, job.Days, job.Location, job.Comments);

            // Tilføj Model til context
            _context.Jobs.Add(newJob);

            // Gem data
            await _context.SaveChangesAsync();

            return returnModel;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobsWithoutExpensesWithoutModels>>> GetModelsWithInfo()
        {
            List<Job> jobs = await _context.Jobs.ToListAsync();

            //laver en liste af models uden udgifter og jobs
            List<JobsWithoutExpensesWithoutModels> jobseliste = new List<JobsWithoutExpensesWithoutModels>();

            // for hver model i modelsDB overføres til vores modelslist af modeller uden udgifter og jobs. 
            foreach (var job in jobs)
            {
                jobseliste.Add(new JobsWithoutExpensesWithoutModels(job.Customer, job.StartDate, job.Days, job.Location, job.Comments));
            }

            return jobseliste.ToList();
        }
        // <summary>
        /// Fetch a job w. models & expenses
        /// </summary>
        /// <param name="id">job Id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Job>> GetJob(long? id)
        {
            //tjek om modellen findes
            var job = await _context.Jobs.FindAsync(id);

            //caster vores expenses til en liste
            /*List<Expense>e1 = */
            _context.Expenses.ToList();
            /*foreach(Expense expense in e1)
                Console.WriteLine(expense);*/
            //model.Expenses = e1.
            if (job == null)
            {
                return NotFound();
            }

            return job;
        }

        // Slet et job:
        /// <summary>
        /// Delete a job by Id
        /// </summary>
        /// <param name="id">Model Id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Job>> Delete(long? id)
        {
            //er id null, så skal der intet gøres.
            if (id == null)
            {
                return NotFound();
            }

            //Find  modellen via ID'et
            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }

            // Slet modellen
            _context.Jobs.Remove(job);
            // Gem modellen
            await _context.SaveChangesAsync();

            // Returner den slettede model
            return job;
        }


        // editer et job:
        /// <summary>
        /// Modify a job by Id
        /// </summary>
        /// <param name="id">Job Id</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<Job>> Edit(long id, string customer, DateTimeOffset startDate, int days, string location, string comments)
        {
            //Find  modellen via ID
            var oldjob = await _context.Jobs.FindAsync(id);
            if (oldjob == null)
            {
                return NotFound();
            }
            //tjek om de forskellige værdier at andet en null
            // for derefter at sætte dem til oldmodel's værdier.
            if (customer != null)
            {
                oldjob.Customer = customer;
            }

            if (startDate != null)
            {
                oldjob.StartDate = startDate;
            }
            if (days != null)
            {
                oldjob.Days = days;
            }
            if (location != null)
            {
                oldjob.Location = location;
            }
            if (comments != null)
            {
                oldjob.Comments = comments;
            }
            
            // Gem ændringerne
            await _context.SaveChangesAsync();

            // Returner den redigerede
            return oldjob;
        }

        [HttpPut("{jobid},{modelid}")]
        public async Task<ActionResult<Job>> EditmodelsOnjob(long jobid, long modelid)
        {
            //Find jobbet via ID
            var oldjob = await _context.Jobs.FindAsync(jobid);
            if (oldjob == null)
            {
                return NotFound();
            }
            
            //Find  modellen via model ID'et
            var oldmodel = await _context.Models.FindAsync(modelid);
            if (oldmodel == null)
            {
                return NotFound();
            }

            long id = oldmodel.ModelId;
            string firstname = oldmodel.FirstName;
            string lastname = oldmodel.LastName;


            List<Model> models = new List<Model>();
            oldjob.Models = models;

            oldjob.Models.Add(oldmodel);
            return oldjob;
        }



        /*
         [HttpDelete("{jobId}")] //slet job
        public async Task<ActionResult<Job>> Delete(long id)
        {
            //Find  modellen via ID'et
            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }
            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{jobId}/{modelId}")] //slet model fra job
        public async Task<ActionResult<Model>> Delete(long? jobId, long? modelId)
        {
            //er id null, så skal der intet gøres.
            if (jobId == null || modelId == null)
            {
                return NotFound();
            }

            //Find  modellen via ID'et
            var job = await _context.Jobs.FindAsync(jobId);
            if (job == null)
            {
                return NotFound();
            }

            Model model = job.Models.Find(x => x.ModelId == modelId);
            job.Models.Remove(model);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet] //Hente en liste med alle jobs.Skal inkludere navn på modeller, som er sat på de enkelte jobs, men ikke expenses
        public async Task<ActionResult<IEnumerable<Job>>> GetJobs()
        {






            return await _context.Jobs.ToListAsync();
        }

        [HttpGet("fuck/{modelId}")] //Hente en liste med alle jobs for en angiven model – uden expenses.
        public async Task<ActionResult<IEnumerable<JobWithoutModelsWithoutExpenses>>> GetJobs(long modelId)
        {
            List<JobWithoutModelsWithoutExpenses> jobWWithoutModelsWithoutExpenseslist = new List<JobWithoutModelsWithoutExpenses>();
            var model = await _context.Models.FindAsync(modelId);
            if (model == null)
            {
                return NotFound();
            }

            foreach (var job in model.Jobs)
            {
                JobWithoutModelsWithoutExpenses jobWithoutModelsWithoutExpenses = new JobWithoutModelsWithoutExpenses(job.Customer, job.StartDate, job.Days, job.Location, job.Comments);
                jobWWithoutModelsWithoutExpenseslist.Add(jobWithoutModelsWithoutExpenses);

            }



            return jobWWithoutModelsWithoutExpenseslist.ToList();
        }

        [HttpGet("{jobId}")] //Hente job med den angivne JobId. Skal inkludere listen med alle expenses for jobbet. 
        public async Task<ActionResult<JobWithoutModels>> GetJobs(long? jobId)
        {
            var job = await _context.Jobs.FindAsync(jobId);
            if (job == null)
            {
                return NotFound();
            }

            JobWithoutModels jobWithoutModels = new JobWithoutModels(job.Customer, job.StartDate, job.Days, job.Location, job.Comments);
            jobWithoutModels.JobId = job.JobId;
            jobWithoutModels.Expenses = job.Expenses;


            return jobWithoutModels;
        }

        [HttpPost("{jobId}/{modelId}")] //Tilføj model til job.
        public async Task<ActionResult<Job>> OnPost(long id, long modelId)
        {

            var model = await _context.Models.FindAsync(modelId);
            var job = await _context.Jobs.FindAsync(id);
            if (job == null || model == null)
            {
                return NotFound();
            }
            else
            {
                model.Jobs.Add(job);
                job.Models.Add(model);
                _context.Jobs.Add(job);
                _context.Models.Add(model);
            }

            await _context.SaveChangesAsync();
            return job;
        }

        [HttpPut] //Opdatere et job
        public async Task<ActionResult<Job>> OnPut(long? id, DateTimeOffset? startDate, int? days, string? location,
            string comments)
        {
            var job = await _context.Jobs.FindAsync(id);

            if (startDate != null)
            {
                job.StartDate = startDate.Value;
            }
            if (days != null)
            {
                job.Days = days.Value;
            }
            if (location != null)
            {
                job.Location = location;
            }
            if (comments != null)
            {
                job.Comments = comments;
            }

            await _context.SaveChangesAsync();
            return job;
        }
        */
    }
}