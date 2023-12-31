﻿using Azure;
using COeX_India1._2.Data;
using COeX_India1._2.Helper;
using COeX_India1._2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
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

        [Authorize]
        [HttpPut("UpdateInventory")]

        public async Task<ActionResult> UpdateInventory(UpdateInventoryModel req)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var claimUserId = claimsIdentity.FindFirst("userId")?.Value;
                var TokenUserId = 0;
                int.TryParse(claimUserId, out TokenUserId);
                var claimUserType = claimsIdentity.FindFirst("userType")?.Value;
                var TokenUserType = Models.User.EUserType.Admin;

                Enum.TryParse(claimUserType, out TokenUserType);
                if (TokenUserType != Models.User.EUserType.SidingUser) { return BadRequest(new Response(false, "Access Denied")); }

                DataHelper dh = new DataHelper();
                List<SqlPara> paras = new List<SqlPara>();
                paras.Add(new SqlPara("@TokenUserId", TokenUserId));
                paras.Add(new SqlPara("@newInventory", req.Inventory));

                string sqlExp = @"
declare @SidingId int
select @SidingId= SidingId from Users where UserId=@TokenUserId

Update Sidings set Inventory= @newInventory where Id=@SidingId
";
                dh.ExecuteNonQuery(sqlExp, paras);

                return Ok(new Response(true, "Inventory Updated"));
            }
            catch(Exception ex)
            {
                return Problem("OOps something went wrong");
            }
        }

    }
}
