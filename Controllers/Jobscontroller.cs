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

        public Jobscontroller(ModelDB context) //context tillader tilgang til databasen
        {
            _context = context;

        }
        /// <summary>
        /// Post new model
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="startDate">StartDate</param>
        /// <param name="days">Days</param>
        /// <param name="location">Location</param>
        /// <param name="comments">Comments</param>
        /// <returns></returns>
        [HttpPost] //opret nyt job
        public async Task<ActionResult<Job>> OnPost(string customer, DateTimeOffset startDate, int days, string location, string comments)
        {
            _context.Jobs.Add(new Job(customer, startDate, days, location, comments)); // tilføjer job til listen i databasen

            await _context.SaveChangesAsync();
            return _context.Jobs.Last();
        }

        /// <summary>
        /// Delete a Job by Id
        /// </summary>
        /// <param name="id">Job Id</param>
        /// <returns></returns>
        [HttpDelete("{jobId}")] //slet job
        public async Task<ActionResult<Job>> Delete(long? id)
        {
            //er id null, så skal der intet gøres.
            if (id == null)
            {
                return NotFound();
            }

            //Find  jobbet via ID'et
            var job = await _context.Jobs.FindAsync(id);
            if (job == null) //hvis jobbet ikke findes returneres notfound.
            {
                return NotFound();
            }
            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
         
            return NoContent();
        }
        /// <summary>
        /// Delete a model from Job by Id
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
            var job = await _context.Jobs.FindAsync(jobId);
            if (job == null)//hvis jobbet ikke findes returneres notfound.
            {
                return NotFound();
            }

            Model model = job.Models.Find(x => x.ModelId == modelId); // finder den korrekte model i jobbet.
            job.Models.Remove(model); //fjerner modellen fra jobbet.
            model.Jobs.Remove(job); //fjerner jobbet fra modellen.
            await _context.SaveChangesAsync();
           
            return NoContent();
        }

        //missing summary needs new version
        //[HttpGet] //Hente en liste med alle jobs.Skal inkludere navn på modeller, som er sat på de enkelte jobs, men ikke expenses
        //public async Task<ActionResult<IEnumerable<Job>>> GetJobs()
        //{
            
        //    //Har andreas lavet

        //    return await _context.Jobs.ToListAsync();
        //}


        /// <summary>
        /// Get all jobs from a model
        /// </summary>
        /// <param name="modelId">Model Id</param>
        /// <returns></returns>
        [HttpGet("Model/{modelId}")] //Hente en liste med alle jobs for en angiven model – uden expenses.
        public async Task<ActionResult<IEnumerable<JobWithoutModelsWithoutExpenses>>> GetJobs(long modelId)
        {
            List<JobWithoutModelsWithoutExpenses> jobWWithoutModelsWithoutExpenseslist = new List<JobWithoutModelsWithoutExpenses>(); //opretter en ny liste til returnering.
            var model = await _context.Models.FindAsync(modelId);
            if (model == null)
            {
                return NotFound();
            }

            foreach (var job in model.Jobs) //opretter nye objekter med korrekt returtype og ligger dem i listen til returnering.
            {
                JobWithoutModelsWithoutExpenses jobWithoutModelsWithoutExpenses = new JobWithoutModelsWithoutExpenses(job.Customer, job.StartDate, job.Days, job.Location, job.Comments);
                jobWWithoutModelsWithoutExpenseslist.Add(jobWithoutModelsWithoutExpenses);

            }



            return jobWWithoutModelsWithoutExpenseslist.ToList();
        }

        /// <summary>
        /// Get job with expenses from ID
        /// </summary>
        /// <param name="jobId">Job Id</param>
        /// <returns></returns>
        [HttpGet("{jobId}")] //Hente job med den angivne JobId. Skal inkludere listen med alle expenses for jobbet. 
        public async Task<ActionResult<JobWithoutModels>> GetJobs(long? jobId)
        {
            var job = await _context.Jobs.FindAsync(jobId);
            if (job == null)
            {
                return NotFound();
            }

            JobWithoutModels jobWithoutModels = new JobWithoutModels(job.Customer, job.StartDate, job.Days, job.Location, job.Comments); //opretter et nyt job med korrekte variabler.
            jobWithoutModels.JobId = job.JobId;       //tilføjer de værdier der ikke er i konstructoren.
            jobWithoutModels.Expenses = job.Expenses;
            

            return jobWithoutModels;
        }
        /// <summary>
        /// Add model by id to job by id
        /// </summary>
        /// <param name="jobId">Job Id</param>
        /// <param name="modelId">Model Id</param>
        /// <returns></returns>
        [HttpPost("{jobId}/{modelId}")] //Tilføj model til job.
        public async Task<ActionResult<Job>> OnPost(long jobId, long modelId)
        {

            var model = await _context.Models.FindAsync(modelId);
            var job = await _context.Jobs.FindAsync(jobId); //finder model og job
            if (job == null || model == null) // validerer at begge eksistere
            {
                return NotFound();
            }
            else
            {
                model.Jobs.Add(job); //ligger model i job og job i model
                job.Models.Add(model);

            }

            await _context.SaveChangesAsync();
            return job;
        }

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
            string comments)
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


    }


}
