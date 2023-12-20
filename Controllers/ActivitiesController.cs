using COeX_India1._2.Data;
using Microsoft.AspNetCore.Mvc;

namespace COeX_India1._2.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ActivitiesController:Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ActivitiesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


    }
}
