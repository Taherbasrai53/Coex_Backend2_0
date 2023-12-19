using COeX_India1._2.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                var sidings = await _dbContext.Sidings.ToListAsync();
                return Ok(sidings); 
            }
            catch(Exception ex)
            {
                return Problem("Oops! plz try again later");
            }
        }

    }
}
