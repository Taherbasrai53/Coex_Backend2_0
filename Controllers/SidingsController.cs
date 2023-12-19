using Azure;
using COeX_India1._2.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Response = COeX_India1._2.Models.Response;


namespace COeX_India1._2.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]

    public class SidingsController:Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public SidingsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [AllowAnonymous]
        [HttpGet("GetAll")]
       
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var sidings = await _dbContext.Sidings.Select(s=> new {s.Id, s.Name, s.Division, s.State}).ToListAsync();
                return Ok(sidings); 
            }
            catch(Exception ex)
            {
                return Problem("Oops! plz try again later");
            }
        }

        [AllowAnonymous]
        [HttpGet("GetById")]

        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest(new Response(false, "Plz enter a valid Id"));
                }
                var siding = await _dbContext.Sidings.Where(s=> s.Id==id).FirstOrDefaultAsync();
                if(siding==null)
                {
                    return BadRequest(new Response(false, "Siding not found"));
                }
                return Ok(siding);
            }
            catch(Exception ex)
            {
                return Problem("Oops! plz try again later");
            }
        }

    }
}
