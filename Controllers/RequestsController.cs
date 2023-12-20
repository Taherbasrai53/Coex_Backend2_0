﻿using COeX_India1._2.Data;
using COeX_India1._2.Helper;
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
                request.Remarks= req.Remarks;

                var activity = new Activities();
                activity.SidingId=request.SidingId;
                activity.Title = "New request added";
                activity.Value = req.Remarks;
                activity.ColorCode = "#b3ffb3";
                activity.Acknowledged = false;

                await _dbContext.Requests.AddAsync(request);
                await _dbContext.Activities.AddAsync(activity); 

                await _dbContext.SaveChangesAsync();
             
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

        [Authorize]
        [HttpGet("GetById")]

        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                var request= await _dbContext.Requests.FirstOrDefaultAsync(r=> r.Id == id);
                return Ok(request);
            }
            catch(Exception ex)
            {
                return Problem("Oops! plz try again later");
            }
        }

        [Authorize]
        [HttpPut("UpdateRequest")]

        public async Task<ActionResult> UpdateRequest(UpdateRequestModel req)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var claimUserId = claimsIdentity.FindFirst("userId")?.Value;
                var TokenUserId = 0;
                int.TryParse(claimUserId, out TokenUserId);
                var claimUserType = claimsIdentity.FindFirst("userType")?.Value;
                var TokenUserType = Models.User.EUserType.Admin;
                


                DataHelper dh = new DataHelper();
                List<SqlPara> paras = new List<SqlPara>();
                paras.Add(new SqlPara("@TokenUserId", TokenUserId));
                paras.Add(new SqlPara("@RequestId", req.Id));
                paras.Add(new SqlPara("@FrieghtAmount", req.FrieghtAmount));
                paras.Add(new SqlPara("@RakesRequired", req.RakesRequired));
                paras.Add(new SqlPara("@RequiredOn", req.RequiredOn));
                paras.Add(new SqlPara("@Remark", req.Remarks));
                paras.Add(new SqlPara("@Reason", req.Reason));             

                string sqlExp = @"
Update Requests set FrieghtAmount= @FrieghtAmount, RakesRequired= @RakesRequired, RequiredOn=@RequiredOn, Remarks=@Remark  where Id=@RequestId

declare @SidingId int
select @SidingId= SidingId from Requests where Id= @RequestId
Insert into Activities (SidingId, Title, Value, ColorCode, Acknowledged) values(@SidingId, 'Request Updated', @Reason, '#ffff99', 0)
";
                dh.ExecuteNonQuery(sqlExp, paras);

                return Ok(new Response(true, "Request updated successfully"));
            }
            catch (Exception ex)
            {
                return Problem("Oops something went wrong");
            }

        }

    }
}
