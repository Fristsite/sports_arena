using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Play.Data;
using Play.Dto.Request;
using Play.Models;

namespace Play.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        public readonly RentDbContext db;
        public readonly IMapper mapper;
        public IConfiguration _configuration;
        public RatingController(RentDbContext db, IMapper mapper, IConfiguration configuration1)
        {
            this.db = db;
            this.mapper = mapper;
            this._configuration = configuration1;
        }
        [Authorize(policy: "User")]
        [HttpPost("Create/Rating")]
        public async Task<IActionResult> Create([FromBody] RatingReq req)
        {

            var lastdatas = await db.Ratings.OrderByDescending(o => o.RatingsId)
                               .FirstOrDefaultAsync();
            long lastids = (lastdatas?.RatingsId + 1) ?? 1;


            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var booking = await db.Bookings.Where(u => u.User.UserName == username).FirstOrDefaultAsync(o => o.BookingId == req.BookingId);

            if (booking == null)
            {
                var resn = new
                {
                    Status = "Failed",
                    Message = "bill id not found or bill id is not yours",
                    data = req
                };
                return BadRequest(resn);

            }


            var newdata = mapper.Map<Ratings>(req);

            newdata.RatingsId = lastids;
            db.SaveChanges();

            var re = mapper.Map<RatingReqs>(newdata);

            var resmodel = new
            {
                Status = "Success",
                Message = "Rating posted Successfully",
                data = re
            };
            return Ok(resmodel);
        }
    }
}