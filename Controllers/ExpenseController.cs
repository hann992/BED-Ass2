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
namespace CountDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
         // Signal R
        private readonly IHubContext<ExpenseHub> _expenseHubContext;

        public ExpenseController(IHubContext<ExpenseHub> expenseHubContext)
        {
            _expenseHubContext = expenseHubContext;
        }

        // GET: api/Count/inc
        [HttpGet("new")]
        public async Task<ActionResult<long>> Get()
        {
            await _expenseHubContext.Clients.All.SendAsync("ReceiveMessage");
            return Ok();
        }
    }
}
