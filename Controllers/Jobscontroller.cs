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
        public async Task<ActionResult<Job>> OnPost(string customer, DateTimeOffset startDate, int days, string location, string comments)
        {
            _context.Jobs.Add(new Job(customer, startDate, days, location, comments));

            await _context.SaveChangesAsync();
            return _context.Jobs.Last();
        }

        [HttpDelete("{jobId}")] //slet job
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
    }
}
