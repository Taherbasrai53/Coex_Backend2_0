using COeX_India1._2.Data;
using COeX_India1._2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace COeX_India1._2.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class RequestsController:Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public RequestsController(ApplicationDbContext context)
        {
            _dbContext=context;   
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddRequests(AddRequestModel req)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var claimUserId = claimsIdentity.FindFirst("userId")?.Value;
                var TokenUserId = 0;
                int.TryParse(claimUserId, out TokenUserId);
                var claimUserType = claimsIdentity.FindFirst("userType")?.Value;
                var TokenUserType = Models.User.EUserType.Admin;
                Console.WriteLine("Hello1");

                Enum.TryParse(claimUserType, out TokenUserType);
                if (TokenUserType != Models.User.EUserType.SidingUser) { return BadRequest(new Response(false, "Access Denied")); }
                Console.WriteLine("Hello2");

                var request = new Requests();
                request.SidingId = (int)await _dbContext.Users.Where(s => s.UserId == TokenUserId).Select(s => s.SidingId).FirstOrDefaultAsync();
                request.FrieghtAmount= req.FrieghtAmount;
                request.RakesRequired = req.RakesRequired;
                request.RequiredOn = req.RequiredOn;
                request.PlacedOn = DateTime.UtcNow;
                request.InsertedBy = TokenUserId;
                Console.WriteLine("Hello3");

                await _dbContext.Requests.AddAsync(request);
                await _dbContext.SaveChangesAsync();

                Console.WriteLine("Hello4");

                return Ok(new Response(true, "Request Added Successfully"));
            }
            catch(Exception ex)
            {
                return Problem("OOps! Something went wrong");
            }
        }

        [Authorize]
        [HttpGet("GetAll")]

        public async Task<ActionResult> GetAllRequests()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var claimUserId = claimsIdentity.FindFirst("userId")?.Value;
            var TokenUserId = 0;
            int.TryParse(claimUserId, out TokenUserId);
            var claimUserType = claimsIdentity.FindFirst("userType")?.Value;
            var TokenUserType = Models.User.EUserType.Admin;
            Console.WriteLine("Hello1");

            Enum.TryParse(claimUserType, out TokenUserType);
            if (TokenUserType != Models.User.EUserType.SidingUser) { return BadRequest(new Response(false, "Access Denied")); }
            var senderId= await _dbContext.Users.Where(s => s.UserId == TokenUserId).Select(s => s.SidingId).FirstOrDefaultAsync();

            var requests = await _dbContext.Requests.Where(r => r.SidingId == senderId && r.Status != Models.Requests.EStatus.completed).OrderByDescending(r => r.PlacedOn).ToListAsync();

            return Ok(requests);

        }

        //[Authorize]
        //[HttpPut("UpdateRequest")]

        //public async Task<ActionResult> UpdateRequest(AddRequestModel req)        
        //{
        //    try
        //    {

        //    }
        //    catch(Exception ex)
        //    {
        //        return Problem 
        //    }

        //}
        
    }
}
