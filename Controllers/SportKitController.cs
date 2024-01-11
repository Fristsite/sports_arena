// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Security.Claims;
// using AutoMapper;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Play.Data;
// using Play.Dto;
// using Play.Dto.Request;
// using Play.Models;

// namespace Play.Reporting
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class ReportController : ControllerBase
//     {

//         public readonly RentDbContext db;
//         public readonly IMapper mapper;
//         public IConfiguration _configuration;

//         public ReportController(RentDbContext db, IMapper mapper, IConfiguration configuration1)
//         {
//             this.db = db;
//             this.mapper = mapper;
//             this._configuration = configuration1;
//         }


//         // [Authorize(policy: "OwnerOrEmployee")]

//         [HttpPost("Get/Arena/Court/Booked/Details1ew")]
//         public async Task<IActionResult> GetReport11([FromBody] date1 date)
//         {

//             var username = User.FindFirst(ClaimTypes.Name)?.Value;

//             var OwnerId = await db.Owners.Where(o => o.UserName == username).Select(o => o.OwnerId).FirstOrDefaultAsync();
//             if (OwnerId == 1)
//             {


//                 var bookcourtData = await db.BookingCourts
//                     .Include(cn => cn.CourtDetails)
//                     .Where(a => a.Status == true)
//                     .Join(
//                         db.Bookings.Include(u => u.User).Include(a => a.Arena),
//                         c => c.BookingId,
//                         m => m.BookingId,
//                         (c, m) => new
//                         {
//                             ArenaName = m.Arena.ArenaName,
//                             GameName = c.CourtDetails.Game.GameName,
//                             CourtName = c.CourtDetails.CourtName,
//                             Date = c.DateToplay,
//                             FromTime = c.FromTime,
//                             ToTime = c.ToTime,
//                             BookingUserId = m.UserId,
//                             Owner = m.Arena.OwnerId
//                         }
//                     )
//                     .Where(data => data.Owner == 1 && data.Date == date.Date)
//                     .ToListAsync();



//                 var reportData = bookcourtData
//                     .GroupBy(o => o.ArenaName)
//                     .Select(grouparena => new
//                     {
//                         arena = grouparena.Key,
//                         Game_Details = grouparena
//                             .GroupBy(o => o.GameName)
//                             .Select(cg => new
//                             {
//                                 gameName = cg.Key,
//                                 Court_Details = cg
//                                     .GroupBy(cd => cd.CourtName)
//                                     .Select(courtGroup => new
//                                     {
//                                         courtName = courtGroup.Key,
//                                         booked_Slots = courtGroup
//                                             .Select(dateGroup => new
//                                             {
//                                                 fromTime = dateGroup.FromTime.ToString("HH:mm:ss"),
//                                                 toTime = dateGroup.ToTime.ToString("HH:mm:ss"),
//                                                 bookingUserName = dateGroup.BookingUserId // Include booking user's name
//                                             })
//                                     })
//                             })
//                     });

//                 return Ok(reportData);
//             }

//             else
//             {
//                 var EmployeeId = await db.Employees.Where(o => o.UserName == username).Select(o => o.ArenaId).FirstOrDefaultAsync();



//                 var bookcourtData = await db.BookingCourts
//                                     .Include(cn => cn.CourtDetails)
//                                     .Where(a => a.Status == true)
//                                     .Join(
//                                         db.Bookings.Include(u => u.User).Include(a => a.Arena),
//                                         c => c.BookingId,
//                                         m => m.BookingId,
//                                         (c, m) => new
//                                         {
//                                             ArenaName = m.Arena.ArenaName,
//                                             GameName = c.CourtDetails.Game.GameName,
//                                             CourtName = c.CourtDetails.CourtName,
//                                             Date = c.DateToplay,
//                                             FromTime = c.FromTime,
//                                             ToTime = c.ToTime,
//                                             BookingUserName = m.User.FullName,
//                                             Owner = m.Arena.ArenaId
//                                         }
//                                     )
//                                     .Where(data => data.Owner == EmployeeId && data.Date == date.Date)
//                                     .ToListAsync();



//                 var reportData = bookcourtData
//                     .GroupBy(o => o.ArenaName)
//                     .Select(grouparena => new
//                     {
//                         arena = grouparena.Key,
//                         Game_Details = grouparena
//                             .GroupBy(o => o.GameName)
//                             .Select(cg => new
//                             {
//                                 gameName = cg.Key,
//                                 Court_Details = cg
//                                     .GroupBy(cd => cd.CourtName)
//                                     .Select(courtGroup => new
//                                     {
//                                         courtName = courtGroup.Key,
//                                         booked_Slots = courtGroup
//                                             .Select(dateGroup => new
//                                             {
//                                                 fromTime = dateGroup.FromTime.ToString("HH:mm:ss"),
//                                                 toTime = dateGroup.ToTime.ToString("HH:mm:ss"),
//                                                 bookingUserName = dateGroup.BookingUserName
//                                             })
//                                     })
//                             })
//                     });

//                 return Ok(reportData);
//             }


//         }


//     }
    
// }




