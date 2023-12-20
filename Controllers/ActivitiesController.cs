using COeX_India1._2.Data;
using COeX_India1._2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;

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

        [HttpGet("GetAll")]
        [Authorize]

        public async Task<ActionResult> GetAllActivities()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var claimUserId = claimsIdentity.FindFirst("userId")?.Value;
            var TokenUserId = 0;
            int.TryParse(claimUserId, out TokenUserId);
            var claimUserType = claimsIdentity.FindFirst("userType")?.Value;
            var TokenUserType = Models.User.EUserType.Admin;
            Enum.TryParse(claimUserType, out TokenUserType);
            if (TokenUserType != Models.User.EUserType.SidingUser) { return BadRequest(new Response(false, "Access Denied")); }

            var sidingId= await _dbContext.Users.Where(u=> u.UserId==TokenUserId).Select(u=> u.SidingId).FirstOrDefaultAsync();

            var activities = await _dbContext.Activities.Where(a=> a.SidingId==sidingId).OrderBy(a => a.InsertedAt).ToListAsync();
            return Ok(activities);
        }                         

            //[HttpPost("send")]
            //public async Task<IActionResult> SendEmail()
            //{
            //    try
            //    {
            //        using (HttpClient httpClient = new HttpClient())
            //        {
            //            httpClient.BaseAddress = new Uri("https://api.smtp2go.com/v3/");
            //            httpClient.DefaultRequestHeaders.Add("api-key", "api-F17A8257010F48D798630CE34C4DA60A");

            //            // Replace with your email content
            //            string subject = "Subject of the email";
            //            string body = "Testing from Coex";
            //            string senderEmail = "en21cs301754@medicaps.ac.in";
            //            string recipientEmail = "taherbasrai5353@gmail.com";

            //            string jsonPayload = $@"
            //        {
            //            "to": ["{recipientEmail}"],
            //            "from": "{senderEmail}",
            //            "subject": "{subject}",
            //            "html": "{body}"
            //        }";

            //            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            //            HttpResponseMessage response = await httpClient.PostAsync("email/send", content);

            //            if (response.IsSuccessStatusCode)
            //            {
            //                return Ok("Email sent successfully!");
            //            }
            //            else
            //            {
            //                string errorMessage = await response.Content.ReadAsStringAsync();
            //                return BadRequest($"Failed to send email: {errorMessage}");
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        return BadRequest($"Failed to send email: {ex.Message}");
            //    }
            //}
        }
    }



