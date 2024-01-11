using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Play.Data;
using Play.Dto;
using Play.Dto.Request;
using Play.Models;

namespace Play.Reporting
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {

        public readonly RentDbContext db;
        public readonly IMapper mapper;
        public IConfiguration _configuration;

        public ReportController(RentDbContext db, IMapper mapper, IConfiguration configuration1)
        {
            this.db = db;
            this.mapper = mapper;
            this._configuration = configuration1;
        }
        [Authorize(policy: "OwnerOrEmployee")]

        [HttpPost("Get/Arena/Court/Booked/Details")]
        public async Task<IActionResult> GetReport11(date1 date)
        {

            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            var owner = await db.Owners.Where(o => o.UserName == username).FirstOrDefaultAsync();

            if (owner != null)
            {
                var ownerId = await db.Owners.Where(o => o.UserName == username).Select(a => a.OwnerId).FirstOrDefaultAsync();

                DateOnly reportDate = date.Date;

                var bookcourtData = await db.BookingCourts
                    .Include(cn => cn.CourtDetails)
                    .Where(a => a.Status == true)
                    .Join(
                        db.Bookings.Include(u => u.User).Include(a => a.Arena),
                        c => c.BookingId,
                        m => m.BookingId,
                        (c, m) => new
                        {
                            ArenaName = m.Arena.ArenaName,
                            GameName = c.CourtDetails.Game.GameName,
                            CourtName = c.CourtDetails.CourtName,
                            Date = c.DateToplay,
                            FromTime = c.FromTime,
                            ToTime = c.ToTime,
                            BookingUserId = m.UserId,
                            Owner = m.Arena.OwnerId
                        }
                    )
                    .Where(data => data.Owner == ownerId && data.Date == reportDate)
                    .ToListAsync();

                var reportData = bookcourtData
                    .GroupBy(o => o.ArenaName)
                    .Select(grouparena => new
                    {
                        arena = grouparena.Key,
                        Game_Details = grouparena
                            .GroupBy(o => o.GameName)
                            .Select(cg => new
                            {
                                gameName = cg.Key,
                                Court_Details = cg
                                    .GroupBy(cd => cd.CourtName)
                                    .Select(courtGroup => new
                                    {
                                        courtName = courtGroup.Key,
                                        booked_Slots = courtGroup
                                            .Select(dateGroup => new
                                            {
                                                fromTime = dateGroup.FromTime.ToString("HH:mm:ss"),
                                                toTime = dateGroup.ToTime.ToString("HH:mm:ss"),
                                                bookingUserName = dateGroup.BookingUserId // Include booking user's name
                                            })
                                    })
                            })
                    });
                var result = new
                {
                    Status = "Success",
                    Message = "All Areana Court Booked Details",
                    Date = reportData

                };
                return Ok(result);
            }
            else
            {
                var ownerId = await db.Employees.Where(o => o.UserName == username).Select(a => a.ArenaId).FirstOrDefaultAsync();

                DateOnly reportDate = date.Date;

                var bookcourtData = await db.BookingCourts
                    .Include(cn => cn.CourtDetails)
                    .Where(a => a.Status == true)
                    .Join(
                        db.Bookings.Include(u => u.User).Include(a => a.Arena),
                        c => c.BookingId,
                        m => m.BookingId,
                        (c, m) => new
                        {
                            ArenaName = m.Arena.ArenaName,
                            GameName = c.CourtDetails.Game.GameName,
                            CourtName = c.CourtDetails.CourtName,
                            Date = c.DateToplay,
                            FromTime = c.FromTime,
                            ToTime = c.ToTime,
                            BookingUserId = m.UserId,
                            ArenaId = m.ArenaId
                        }
                    )
                    .Where(data => data.ArenaId == ownerId && data.Date == reportDate)
                    .ToListAsync();

                var reportData = bookcourtData
                    .GroupBy(o => o.ArenaName)
                    .Select(grouparena => new
                    {
                        arena = grouparena.Key,
                        Game_Details = grouparena
                            .GroupBy(o => o.GameName)
                            .Select(cg => new
                            {
                                gameName = cg.Key,
                                Court_Details = cg
                                    .GroupBy(cd => cd.CourtName)
                                    .Select(courtGroup => new
                                    {
                                        courtName = courtGroup.Key,
                                        booked_Slots = courtGroup
                                            .Select(dateGroup => new
                                            {
                                                fromTime = dateGroup.FromTime.ToString("HH:mm:ss"),
                                                toTime = dateGroup.ToTime.ToString("HH:mm:ss"),
                                                bookingUserName = dateGroup.BookingUserId // Include booking user's name
                                            })
                                    })
                            })
                    });
                var result = new
                {
                    Status = "Success",
                    Message = "Areana Court Booked Details",
                    Date = reportData

                };
                return Ok(result);
            }




        }
        [Authorize(policy: "OwnerOrEmployee")]

        [HttpPost("Get/Arena/SportKit/Booked/Details")]
        public async Task<IActionResult> GetReport111(date1 date)
        {

            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            var owner = await db.Owners.Where(o => o.UserName == username).FirstOrDefaultAsync();

            if (owner != null)
            {
                var ownerId = await db.Owners.Where(o => o.UserName == username).Select(a => a.OwnerId).FirstOrDefaultAsync();

                DateOnly reportDate = date.Date;

                var bookcourtData = await db.BookingSportskits
                    .Include(cn => cn.SportsKit)
                    .Where(a => a.Status == true)
                    .Join(
                        db.Bookings.Include(u => u.User).Include(a => a.Arena),
                        c => c.BookingId,
                        m => m.BookingId,
                        (c, m) => new
                        {
                            ArenaName = m.Arena.ArenaName,
                            Sport_kit_Name = c.SportsKit.Game.GameName,
                            Date = c.DateToplay,
                            FromTime = c.FromTime,
                            ToTime = c.ToTime,
                            BookingUserId = m.User.UserName,
                            Owner = m.Arena.OwnerId
                        }
                    )
                    .Where(data => data.Owner == ownerId && data.Date == reportDate)
                    .ToListAsync();

                var reportData = bookcourtData
                    .GroupBy(o => o.ArenaName)
                    .Select(grouparena => new
                    {
                        arena = grouparena.Key,
                        Sport_Kit_Details = grouparena
                            .GroupBy(o => o.Sport_kit_Name)
                            .Select(cg => new
                            {
                                Kit_Name = cg.Key,
                                Booked_slots = cg.Select(s => new
                                {
                                    From_Time = s.FromTime.ToString("HH:mm:ss"),
                                    To_Time = s.ToTime.ToString("HH:mm:ss"),
                                    Booked_User_Name = s.BookingUserId
                                })
                            })
                    });
                var result = new
                {
                    Status = "Success",
                    Message = "All Areana Sportkit Booked Details",
                    Date = reportData

                };
                return Ok(result);
            }
            else
            {
                var ownerId = await db.Employees.Where(o => o.UserName == username).Select(a => a.ArenaId).FirstOrDefaultAsync();

                DateOnly reportDate = date.Date;

                var bookcourtData = await db.BookingSportskits
                    .Include(cn => cn.SportsKit)
                    .Where(a => a.Status == true)
                    .Join(
                        db.Bookings.Include(u => u.User).Include(a => a.Arena),
                        c => c.BookingId,
                        m => m.BookingId,
                        (c, m) => new
                        {
                            ArenaName = m.Arena.ArenaName,
                            Sport_kit_Name = c.SportsKit.Game.GameName,
                            Date = c.DateToplay,
                            FromTime = c.FromTime,
                            ToTime = c.ToTime,
                            BookingUserId = m.User.UserName,
                            Owner = m.Arena.OwnerId
                        }
                    )
                    .Where(data => data.Owner == ownerId && data.Date == reportDate)
                    .ToListAsync();

                var reportData = bookcourtData
                    .GroupBy(o => o.ArenaName)
                    .Select(grouparena => new
                    {
                        arena = grouparena.Key,
                        Sport_Kit_Details = grouparena
                            .GroupBy(o => o.Sport_kit_Name)
                            .Select(cg => new
                            {
                                Kit_Name = cg.Key,
                                Booked_slots = cg.Select(s => new
                                {
                                    From_Time = s.FromTime.ToString("HH:mm:ss"),
                                    To_Time = s.ToTime.ToString("HH:mm:ss"),
                                    Booked_User_Name = s.BookingUserId
                                })
                            })
                    });
                var result = new
                {
                    Status = "Success",
                    Message = "Areana Sportkit Booked Details",
                    Date = reportData

                };
                return Ok(result);
            }



        }

        [HttpGet("MostBookedTimeSlots")]
        public async Task<IActionResult> GetMostBookedTimeSlots(int year, int month)
        {
            try
            {
                var startDate = DateOnly.FromDateTime(new DateTime(year, month, 1));
                var endDate = startDate.AddMonths(1).AddDays(-1);

                var mostBookedTimeSlots = await db.BookingCourts
                    .Where(booking => booking.DateToplay >= startDate && booking.DateToplay <= endDate)
                    .GroupBy(booking => new { booking.DateToplay.Day, booking.FromTime })
                    .Select(group => new
                    {
                        Day = group.Key.Day,
                        TimeSlot = group.Key.FromTime,
                        BookingCount = group.Count()
                    })
                    .OrderByDescending(result => result.BookingCount)
                    .FirstOrDefaultAsync();

                if (mostBookedTimeSlots != null)
                {
                    var reportData = new
                    {
                        Day = mostBookedTimeSlots.Day,
                        TimeSlot = mostBookedTimeSlots.TimeSlot.ToString("hh\\:mm tt"),
                        BookingCount = mostBookedTimeSlots.BookingCount
                    };
                    var result = new
                    {
                        Status = "Success",
                        Message = "Most Of Booked Slots",
                        Date = reportData

                    };
                    return Ok(result);
                }
                else
                {
                    return NotFound("No bookings found for the specified month.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }






        [Authorize(Policy = "User")]
        [HttpGet("PastOneMontUserBookingList")]
        public async Task<IActionResult> pastBooking()
        {
            var curmonth = DateTime.Now.Month;
            var lastmonth = DateTime.Now.AddMonths(-1).Month;

            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            var UserId = await db.Users.Where(o => o.UserName == username).Select(o => o.UserId).FirstOrDefaultAsync();

            var court = await db.Bookings
                .Where(a => a.BookingCartTypeId == 1 && a.UserId == UserId && a.DateOfBooked.Month >= lastmonth && a.DateOfBooked.Month <= curmonth)
                .ToListAsync();

            var courtDetails = court
                .Select(group => new
                {
                    Booking_Id = group.BookingId,
                    Booked_date = group.DateOfBooked,
                    Total_BiLL_Amount = group.TotalAmount,
                    Booking_Status = group.BookedStatus,
                    Booking_cart_details = db.BookingCourts
                        .Where(b => b.BookingId == group.BookingId)
                        .Select(u => new
                        {
                            u.CourtDetails.CourtName,
                            u.DateToplay,
                            u.FromTime,
                            u.ToTime,
                            u.TotalAmount
                        })
                }).ToList();

            var Kit = await db.Bookings
                .Where(a => a.BookingCartTypeId == 2 && a.UserId == UserId && a.DateOfBooked.Month >= lastmonth && a.DateOfBooked.Month <= curmonth)
                .ToListAsync();

            var kitDetails = Kit
                .Select(group => new
                {
                    Booking_Id = group.BookingId,
                    Booked_date = group.DateOfBooked,
                    Total_BiLL_Amount = group.TotalAmount,
                    Booking_Status = group.BookedStatus,
                    Booking_cart_details = db.BookingSportskits
                        .Where(b => b.BookingId == group.BookingId)
                        .Select(u => new
                        {
                            u.SportsKit.Game.GameName,
                            u.DateToplay,
                            u.FromTime,
                            u.ToTime,
                            u.TotalAmount
                        })
                }).ToList();

            var res = new
            {
                Status = "Success",
                Message = "Booking Details of past month",
                Court_Bookings = courtDetails,
                SportsKit_Bookings = kitDetails
            };

            return Ok(res);
        }
        [HttpGet("NewUserList")]
        public async Task<IActionResult> NewUsers()
        {
            var curmonth = DateTime.Now.Month;
            var lastmonth = DateTime.Now.AddMonths(-1).Month;



            var court = await db.Users
                .Where(a => a.CreatedAt.Month >= lastmonth && a.CreatedAt.Month <= curmonth).Select(o => new
                {
                    o.FullName,
                    o.PhoneNumber
                })
                .ToListAsync();

            var res = new
            {
                Status = "Success",
                Message = "New User List",
                UserList = court
            };

            return Ok(res);





        }
        [Authorize(policy: "User")]
        [HttpGet("UserPaymentHistory")]
        public async Task<IActionResult> NewUserss()
        {
            var curmonth = DateTime.Now.Month;
            var lastmonth = DateTime.Now.AddMonths(-1).Month;

            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var userId = await db.Users.Where(o => o.UserName == username).Select(o => o.UserId).FirstOrDefaultAsync();



            var court = await db.Payments.Include(o => o.Booking)
                .Where(a => a.PaymentDate.Month >= lastmonth && a.Booking.UserId == userId && a.PaymentDate.Month <= curmonth).Select(o => new
                {
                    BookingID = o.BookingId,
                    Paid_Date = o.PaymentDate,
                    Paid_Amount = o.Amount,
                    Payment_Method = o.PaymentMethod,
                    Transaction_Id = o.TransactionId,
                    Status = o.PaymentStatus


                })
                .ToListAsync();

            var res = new
            {
                Status = "Success",
                Message = "Payment Histort For Past one month",
                UserList = court
            };

            return Ok(res);
        }
        [Authorize(Policy = "User")]
        [HttpGet("DiscountCountAndTotalAmount")]
        public async Task<IActionResult> GetDiscountCountAndTotalAmount()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var userId = await db.Users.Where(o => o.UserName == username).Select(o => o.UserId).FirstOrDefaultAsync();

            var totalAmount = await db.Bookings
                .Where(a => a.UserId == userId)
                .SumAsync(a => a.TotalAmount);


            var discountCount = await db.Bookings
                .Where(a => a.UserId == userId && a.DiscountAmount > 0)
                .CountAsync();

            var result = new
            {
                Status = "Success",
                Message = "DiscountDetails",

                Discount_Count = discountCount,
                TotalAmount = totalAmount

            };





            return Ok(result);
        }

        [HttpGet("MostOfPlayedUserInAMonth")]
        public async Task<IActionResult> GetMostOfPlayedUserInAMonth()
        {

            var curmonth = DateTime.Now.Month;
            var lastmonth = DateTime.Now.AddMonths(-1).Month;



            int minPlayCount = 5;

            var personPlayCounts = await db.PersonToPlays.Include(o => o.Booking).ThenInclude(a => a.BookingCourts)
                .GroupBy(p => p.Person_Name)
                .Where(group => group.Count() > minPlayCount)
                .Select(group => new
                {

                    PersonName = group.Key,
                    PlayCount = group.Count()
                })
                .ToListAsync();




            var result = new
            {
                Status = "Success",
                Message = "Person_Who_Played_Five_Times_More",
                Player_Details = personPlayCounts


            };





            return Ok(result);
        }

        [HttpPost("MostOfPlayedUserInAMonth")]
        public async Task<IActionResult> CountOfUser(IDReq Id)
        {

            if (Id.Id == 1)
            {
                var curmonth = DateTime.Now.Month;
                var lastmonth = DateTime.Now.AddMonths(-1).Month;
                var topThree = await db.Bookings.Include(o => o.User)
                    .Where(n => n.DateOfBooked.Month >= lastmonth && n.DateOfBooked.Month <= curmonth)
                    .GroupBy(a => a.User.UserName)
                    .Select(group => new
                    {
                        User_Name = group.Key,
                        Count1 = group.Count()
                    })
                    .OrderByDescending(a => a.Count1)
                    .Take(3)
                    .ToListAsync();


                var result = new
                {
                    Status = "Success",
                    Message = "Top Three Persons Booked list with Count",
                    Player_Details = topThree


                };
                return Ok(result);
            }
            else if (Id.Id == 2)
            {
                var usersWithMoreThanFiveHours = await db.Bookings
                                                    .GroupBy(b => b.UserId)
                                                        .Select(group => new
                                                        {
                                                            UserId = group.Key,
                                                            TotalBookingHours = group.Sum(b => b.TotalAmount)
                                                        })
                                                        .Where(user => user.TotalBookingHours > 5)
                                                        .ToListAsync();


                if (usersWithMoreThanFiveHours == null)
                {

                    var result = new
                    {
                        Status = "Failed",
                        Message = "users With More Than Five Hours details not found",
                        Player_Details = usersWithMoreThanFiveHours


                    };
                    return NotFound(result);
                }
                var results = new
                {
                    Status = "Success",
                    Message = "users With More Than Five Hours",
                    Player_Details = usersWithMoreThanFiveHours


                };
                return Ok(results);

            }
            else
            {
                var curmonth = DateTime.Now.Month;
                var lastmonth = DateTime.Now.AddMonths(-1).Month;
                var topThree = await db.Bookings.Include(o => o.User)
                    .Where(n => n.DateOfBooked.Month >= lastmonth && n.DateOfBooked.Month <= curmonth)
                    .GroupBy(a => a.User.UserName)
                    .Select(group => new
                    {
                        User_Name = group.Key,
                        Count1 = group.Count()
                    })
                    .OrderByDescending(a => a.Count1)
                    .ToListAsync();


                var result = new
                {
                    Status = "Success",
                    Message = "Persons Booked list with Count",
                    Player_Details = topThree


                };
                return Ok(result);
            }







        }
        [Authorize(policy:"Owner")]
        [HttpPost("PersonsPlayedListForParticularBookingId")]
        public async Task<IActionResult> GetPersonsPlayedListForParticularBookingId(IDReq id)
        {
            var persons = await db.PersonToPlays.Where(o => o.BookingId == id.Id).Select(a => a.Person_Name).ToListAsync();
            var count = persons.Count();











            var result = new
            {
                Status = "Success",
                Message = "Persons Played List For Particular BookingId",
                Player_Count = count,
                Player_Details = persons



            };





            return Ok(result);
        }




    }



}


