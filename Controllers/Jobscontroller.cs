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

        [HttpPost]
        public async Task<ActionResult<Job>> OnPost(string customer, DateTimeOffset startDate, int days, string location, string comments)
        {
            //dette virker helt fint dog med 


           // Model model = _context.Models.Find(x => x.ModelId.contains(modelId));

            _context.Jobs.Add(new Job(customer, startDate, days, location, comments));

            await _context.SaveChangesAsync();
            return _context.Jobs.Last();
        }

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
            //da den stadig er i ram, så vil jeg gerne se hvilken 
            //som er slettet.
            return job;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Job>>> GetJobs()
        {
            return await _context.Jobs.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Job>> GetJob(long? id)
        {
            var model = await _context.Jobs.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            return model;
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<Job>> OnPost(long id, long modelId)
        {
            //dette virker helt fint dog med jaja


            // Model model = _context.Models.Find(x => x.ModelId.contains(modelId));
            
            var model = await _context.Models.FindAsync(modelId);
            var job = await _context.Jobs.FindAsync(id);
            if (job == null || model == null)
            {
                return NotFound();
            }
            else
            {
                //model.Jobs.Add(job);
                job.Models.Add(model);
            }

            await _context.SaveChangesAsync();
            return job;
        }



    }
}
