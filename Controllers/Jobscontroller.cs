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


        // #1

        /// <summary>
        /// Create new Job
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="startDate">StartDate</param>
        /// <param name="days">Days</param>
        /// <param name="location">Location</param>
        /// <param name="comments">Comments</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<JobSimple>> OnPost(string customer, DateTimeOffset startDate, int days, string location, string comments)
        {
            // Laver et nyt job, det som skal gemmes
            Job newJob = new Job(customer, startDate, days, location, comments);

            // Vi tilføjer det nye job til contexten.
            _context.Jobs.Add(newJob);

            // Vi gemmer det nye job
            await _context.SaveChangesAsync();

            // Vi finder det gemte ID, så retur Job reflektere det ægte ID
            newJob.JobId = _context.Jobs.Last().JobId;

            // Laver et DTO uden lister som skal returneres:
            JobSimple returnJob = new JobSimple(newJob);

            return returnJob;
        }


        // #2

        /// <summary>
        /// Delete a Job by Id
        /// </summary>
        /// <param name="id">Job Id</param>
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
            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();


            return job;
        }

        // #3

        /// <summary>
        /// update job
        /// </summary>
        /// <param name="id">Job Id</param>
        /// <param name="startDate">StartDate</param>
        /// <param name="days">Days</param>
        /// <param name="location">Location</param>
        /// <param name="comments">Comments</param>
        /// <returns></returns>
        [HttpPut] //Opdatere et job
        public async Task<ActionResult<Job>> OnPut(long? id, DateTimeOffset? startDate, int? days, string? location,
            string? comments)
        {
            var job = await _context.Jobs.FindAsync(id); // finder job.
            if (job == null)
            {
                return NotFound();
            }
            //Tjekker om variablerne er blevet indsat til at blive udskiftet, hvis de er bliver de udskiftet.
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

        // #4

        /// <summary>
        /// Add model by id to job by id
        /// </summary>
        /// <param name="jobId">Job Id</param>
        /// <param name="modelId">Model Id</param>
        /// <returns></returns>
        [HttpPut("{jobId}/{modelId}")]
        public async Task<IActionResult> PutJobModel(long jobId, long modelId)
        {
            var job = await _context.Jobs.Where(x => x.JobId == jobId).Include(m => m.Models).FirstOrDefaultAsync();
            var model = await _context.Models.Where(x => x.ModelId == modelId).Include(j => j.Jobs).FirstOrDefaultAsync();

            job.Models.Add(model);
            model.Jobs.Add(job);

            _context.Entry(job).State = EntityState.Modified;
            _context.Entry(model).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }


        // #5

        /// <summary>
        /// Delete a model by id from Job by Id
        /// </summary>
        /// <param name="jobId">Job Id</param>
        /// <param name="modelId">Model Id</param>
        /// <returns></returns>
        [HttpDelete("{jobId}/{modelId}")] //slet model fra job
        public async Task<ActionResult<Model>> Delete(long? jobId, long? modelId)
        {
            //er id null, så skal der intet gøres.
            if (jobId == null || modelId == null)
            {
                return NotFound();
            }

            //Find  jobbet via ID'et
            var job = await _context.Jobs.Include(x => x.Models).Where(x => x.JobId == jobId).FirstAsync();
            if (job == null)//hvis jobbet ikke findes returneres notfound.
            {
                return NotFound();
            }
            if (job.Models == null)//hvis jobbet ikke findes returneres notfound.
            {
                return NotFound();
            }
            Model model = job.Models.Find(x => x.ModelId == modelId); // finder den korrekte model i jobbet.

            job.Models.Remove(model); //fjerner modellen fra jobbet.
            
            model.Jobs.Remove(job); //fjerner jobbet fra modellen.

            _context.Entry(job).State = EntityState.Modified;
            _context.Entry(model).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // #6

        /// <summary>
        /// Get all Jobs, with models, without expenses
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobWithModels>>> GetJobs()
        {
            List<Job> jobs = await _context.Jobs.Include(x => x.Models).ToListAsync();

            List<JobWithModels> jobsWithModels = new List<JobWithModels>();

            foreach (Job job in jobs)
            {
                List<ModelWithoutExpensesWithoutJobs> modelsList = new List<ModelWithoutExpensesWithoutJobs>();
                foreach (var model in job.Models)
                {
                    modelsList.Add(new ModelWithoutExpensesWithoutJobs(model.ModelId, model.FirstName, model.LastName, model.Email, model.PhoneNo, model.AddresLine1, model.AddresLine2, model.Zip, model.City, model.BirthDay, model.Height, model.ShoeSize, model.HairColor, model.Comments));
                }
                JobWithModels newJobWithModels = new JobWithModels(job);
                newJobWithModels.Models = modelsList;

                jobsWithModels.Add(newJobWithModels);
            }

            return jobsWithModels.ToList();
        }

        // #7

        /// <summary>
        /// Get all jobs from a model
        /// </summary>
        /// <param name="modelId">Model Id</param>
        /// <returns></returns>
        [HttpGet("Model/{modelId}")] //Hente en liste med alle jobs for en angiven model – uden expenses.
        public async Task<ActionResult<IEnumerable<JobWithoutModelsWithoutExpenses>>> GetJobs(long modelId)
        {
            //opretter en ny liste til returnering.
            List<JobWithoutModelsWithoutExpenses> newJobList = new List<JobWithoutModelsWithoutExpenses>(); 


            var model = await _context.Models.Where(x => x.ModelId == modelId).Include(x => x.Jobs).FirstAsync();

            
            if (model == null)
            {
                return NotFound();
            }

            if (model.Jobs == null)
            {
                return NotFound();
            }

            foreach (var job in model.Jobs) //opretter nye objekter med korrekt returtype og ligger dem i listen til returnering.
            {
                Console.WriteLine("Job for " + model.FirstName + ": " + job.Customer);
                JobWithoutModelsWithoutExpenses newJob = new JobWithoutModelsWithoutExpenses(job.Customer, job.StartDate, job.Days, job.Location, job.Comments);
                newJob.JobId = job.JobId;
                newJobList.Add(newJob);

            }



            return newJobList.ToList();
        }


        
        // #8

        /// <summary>
        /// Get job from ID, with expenses
        /// </summary>
        /// <param name="jobId">Job Id</param>
        /// <returns></returns>
        [HttpGet("{jobId}")] //Hente job med den angivne JobId. Skal inkludere listen med alle expenses for jobbet. 
        public async Task<ActionResult<JobWithoutModels>> GetJobs(long? jobId)
        {
            var job = await _context.Jobs.Where(x => x.JobId == jobId).Include(x => x.Expenses).FirstAsync();

            Console.WriteLine("Expenses: " + job.Expenses.Count);

            if (job == null)
            {
                return NotFound();
            }
            
            JobWithoutModels jobWithoutModels = new JobWithoutModels(job.Customer, job.StartDate, job.Days, job.Location, job.Comments); //opretter et nyt job med korrekte variabler.
            
            jobWithoutModels.JobId = job.JobId;       //tilføjer de værdier der ikke er i konstructoren.
            jobWithoutModels.Expenses = job.Expenses;



            return jobWithoutModels;
        }





    }
}
