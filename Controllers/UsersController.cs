using COeX_India1._2.Data;
using Microsoft.AspNetCore.Mvc;

namespace COeX_India1._2.Controllers
{
    public class UsersController:Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public UsersController(ApplicationDbContext dbContext)
        {
            _dbContext=dbContext;
        }
    }
}
