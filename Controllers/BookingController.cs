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
using Play.Dto;
using Play.Dto.Request;
using Play.Models;

namespace Play.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        public readonly RentDbContext db;
        public readonly IMapper mapper;
        public IConfiguration _configuration;
        public BookingController(RentDbContext db, IMapper mapper, IConfiguration configuration1)
        {
            this.db = db;
            this.mapper = mapper;
            this._configuration = configuration1;
        }


        [Authorize(Policy = "User")]
        [HttpPost("Create/Booking")]
        public async Task<IActionResult> Booking([FromBody] BookingReq b)
        {

            var lastdata = await db.Bookings.OrderByDescending(o => o.BookingId)
                                   .FirstOrDefaultAsync();
            long lastid = (lastdata?.BookingId + 1) ?? 1;


            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var userId = await db.Users
                        .Where(o => o.UserName == username)
                        .Select(a => a.UserId)
                        .FirstOrDefaultAsync();
            var date = DateOnly.FromDateTime(DateTime.Now);


            var newData = new Booking
            {
                BookingId = lastid,
                UserId = userId,
                BookingCartTypeId = b.BookingCartTypeId,
                ArenaId = b.ArenaId,
                DateOfBooked = DateTime.Now.ToUniversalTime(),
                BookedStatus = false,
                SubTotal = 0,
                DiscountAmount = 0,
                TotalAmount = 0,
            };

            await db.Bookings.AddAsync(newData);
            await db.SaveChangesAsync();

            var resdata = mapper.Map<Bokreq>(newData);

            if (newData.BookingCartTypeId == 1)
            {
                var res = new
                {
                    Status = "Success",
                    Message = "Booking id is created for court booking",
                    data = resdata
                };
                return Ok(res);
            }
            var ress = new
            {
                Status = "Success",
                Message = "Booking id is created for Sports kit booking",
                data = resdata
            };
            return Ok(ress);




        }
        [Authorize("User")]
        [HttpPost("Create/BookingCourt")]
        public async Task<IActionResult> BookingCourt([FromBody] BookingCourtreq i)
        {
            bool IsCourtAvailable(long courtDetailsId, TimeOnly fromTime, TimeOnly toTime)
            {
                TimeOnly workingHoursStart = new TimeOnly(6, 0, 0); // 06:00:00
                TimeOnly workingHoursEnd = new TimeOnly(22, 0, 0);  // 22:00:00

                if (fromTime < workingHoursStart || toTime > workingHoursEnd)
                {
                    return false;
                }

                bool isAvailable = !db.BookingCourts
                    .Any(bookingCart =>
                        bookingCart.ItemId == courtDetailsId &&
                        bookingCart.Status == true &&
                        bookingCart.FromTime < toTime &&
                        bookingCart.ToTime > fromTime
                    );

                return isAvailable;
            }

            var book = db.Bookings.Where(o => o.BookingCartTypeId == 1).FirstOrDefault(b => b.BookingId == i.BookingId);

            if (book == null)
            {
                var ress = new
                {
                    Status = "Failed",
                    Message = "Booking id is not created for court booking",
                    data = i
                };
                return Ok(ress);
            }
            if (book.BookedStatus == true)
            {
                var ress = new
                {
                    Status = "Failed",
                    Message = "Already booked. Court not added to cart",
                    data = i
                };
                return Ok(ress);
            }

            var toTime = i.FromTime.AddHours(i.DurationInHours).AddMinutes(-1);

            // Check if a court booking with the same BookingId already exists
            var existingCourtBooking = db.BookingCourts.Where(o=>o.ItemId==i.ItemId && o.FromTime == i.FromTime).FirstOrDefault(c => c.BookingId == i.BookingId);
            if (existingCourtBooking != null)
            {
                var res = new
                {
                    Status = "Failed",
                    Message = "A court booking for the same BookingId already exists",
                    data = i
                };
                return BadRequest(res);
            }

            if (IsCourtAvailable(i.ItemId, i.FromTime, toTime) == true)
            {
                var lastdatas = await db.BookingCourts.OrderByDescending(o => o.BookingCartId)
                           .FirstOrDefaultAsync();
                long lastids = (lastdatas?.BookingCartId + 1) ?? 1;

                var price = await db.CourtDetails.Where(o => o.ItemId == i.ItemId).FirstOrDefaultAsync();
                if (price == null)
                {
                    return BadRequest(new { Status = "Failed", Message = "Court ItemId not found", Data = i });
                }
                var totalprice = price.Price * i.DurationInHours;

                var newdata = new BookingCourt
                {
                    BookingCartId = lastids,
                    BookingId = i.BookingId,
                    ItemId = i.ItemId,
                    DateToplay = i.DateToplay,
                    FromTime = i.FromTime,
                    TotalAmount = totalprice,
                    Status = false,
                    ToTime = toTime,
                };

                await db.BookingCourts.AddAsync(newdata);
                await db.SaveChangesAsync();
                var res = new
                {
                    Status = "Success",
                    Message = "Court is added to Cart",
                    data = i
                };
                return Ok(res);
            }
            else
            {
                var res = new
                {
                    Status = "Failed",
                    Message = "Court is not available during your entered time. Please enter a time within the working hours",
                    data = i
                };
                return BadRequest(res);
            }
        }

        [Authorize(policy: "User")]
        [HttpPost("Create/Bookingsportskit")]
        public async Task<IActionResult> Bookingsportskit([FromBody] BookingSportskitreq i)
        {

            bool IsSportsKitAvailable(long sportsKitId, DateOnly dateToPlay, TimeOnly fromTime, TimeOnly toTime)
            {

                TimeOnly workingHoursStart = new TimeOnly(6, 0, 0); // 06:00:00
                TimeOnly workingHoursEnd = new TimeOnly(22, 0, 0);  // 22:00:00

                if (fromTime < workingHoursStart || toTime > workingHoursEnd)
                {
                    return false;
                }

                var bookedSlots = db.BookingSportskits
                     .Where(booking =>
                         booking.ItemId == sportsKitId &&
                         booking.Status == true &&
                         booking.DateToplay == dateToPlay &&
                         booking.FromTime < toTime &&
                         booking.ToTime > fromTime
                     )
                     .ToList();

                var sportsKit = db.SportsKits
                    .Where(kit =>
                        kit.ItemId == sportsKitId &&
                        kit.IsActive == true)
                    .FirstOrDefault();

                if (sportsKit == null)
                {
                    return false;
                }

                int maxBookings = sportsKit.Count;

                int availableCount = maxBookings - bookedSlots.Count;

                return availableCount > 0;
            }


            var book = db.Bookings.Where(o => o.BookingCartTypeId == 2).FirstOrDefault(b => b.BookingId == i.BookingId);

            if (book == null)
            {
                var ress = new
                {
                    Status = "Failed",
                    Message = "Booking id is not created for sportskit booking",
                    data = i
                };
                return Ok(ress);
            }

            if (book.BookedStatus == true)
            {
                var ress = new
                {
                    Status = "Failed",
                    Message = "already booked. sportskit not add to card",
                    data = i
                };
                return Ok(ress);
            }




            var toTime = i.FromTime.AddHours(i.DurationInHours).AddMinutes(-1);

            var existingCourtBooking = db.BookingSportskits.FirstOrDefault(c => c.BookingId == i.BookingId);
            if (existingCourtBooking != null)
            {
                var res = new
                {
                    Status = "Failed",
                    Message = "A sportskit booking for the same BookingId already exists",
                    data = i
                };
                return BadRequest(res);
            }
            if (IsSportsKitAvailable(i.ItemId, i.DateToplay, i.FromTime, toTime) == true)
            {
                var lastdatas = await db.BookingSportskits.OrderByDescending(o => o.BookingSportskitId)
                                           .FirstOrDefaultAsync();
                long lastids = (lastdatas?.BookingSportskitId + 1) ?? 1;

                var price = await db.SportsKits.Where(o => o.ItemId == i.ItemId).FirstOrDefaultAsync();
                if (price == null)
                {
                    return BadRequest(new
                    {
                        Status = "Failed",
                        Message = "Sportskit ItemId not fount",
                        Data = i
                    });
                }
                var totalprice = price.PricePerHour * i.DurationInHours;

                var newdata = new BookingSportskit
                {
                    BookingSportskitId = lastids,
                    BookingId = i.BookingId,

                    ItemId = i.ItemId,
                    DateToplay = i.DateToplay,
                    FromTime = i.FromTime,
                    TotalAmount = totalprice,
                    Status = false,
                    ToTime = i.FromTime.AddHours(i.DurationInHours).AddMinutes(-1),

                };
                await db.BookingSportskits.AddAsync(newdata);
                await db.SaveChangesAsync();
                var res = new
                {
                    Status = "Success",
                    Message = "SportsKit is added to Cart",
                    data = i
                };
                return Ok(res);
            }
            else
            {
                var res = new
                {
                    Status = "Failed",
                    Message = "sportskit is not Available",
                    data = i
                };
                return BadRequest(res);
            }


        }

        [Authorize(Policy = "User")]
        [HttpGet("Get/Bill_Amount/Booking/id")]
        public async Task<IActionResult> GetBillAmount([FromBody] IDReq id)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var booking = await db.Bookings.Include(a => a.User).Where(u => u.User.UserName == username).FirstOrDefaultAsync(o => o.BookingId == id.Id);

            if (booking == null)
            {
                var resn = new
                {
                    Status = "Failed",
                    Message = "bill id not found or bill id is not yours",
                    data = id.Id
                };
                return BadRequest(resn);
            }

            double total = 0;
            if (booking.BookingCartTypeId == 1)
            {
                var bc = await db.BookingCourts.Where(u => u.BookingId == id.Id).ToListAsync();
                var bcm = mapper.Map<List<Bc>>(bc); // Use AutoMapper to map BookingCourt entities to Bc DTOs
                total = bcm.Sum(o => o.TotalAmount);
            }
            else
            {
                var bc = await db.BookingSportskits.Where(u => u.BookingId == id.Id).ToListAsync();
                var bcm = mapper.Map<List<Bc>>(bc); // Use AutoMapper to map BookingSportskit entities to Bc DTOs
                total = bcm.Sum(o => o.TotalAmount);
            }


            // discout
            double discout = 0;
            var user = db.Bookings.Where(o => o.User.UserName == username && o.DateOfBooked.Month == DateTime.Now.Month).Count();
            if (user >= 3)
            {
                discout = (double)total * (10 / 100);

            }
            var use = db.Bookings.Where(o => o.User.UserName == username).Count();
            if (use == 1)
            {
                discout = (double)total * (20 / 100);
            }
            // ... Rest of your discount calculation code

            booking.SubTotal = total;
            booking.DiscountAmount = discout;
            booking.TotalAmount = total - discout;

            await db.SaveChangesAsync();

            if (booking.BookingCartTypeId == 1)
            {
                var bc = await db.BookingCourts.Where(u => u.BookingId == id.Id).ToListAsync();
                var bcm = mapper.Map<List<Bc>>(bc);

                var br = mapper.Map<Br>(booking);

                br.Cart_Items = bcm;

                var res = new
                {
                    Status = "Success",
                    Message = "Booking Details with Total_Amount",
                    data = br
                };
                return Ok(res);
            }
            else
            {
                var bc = await db.BookingSportskits.Where(u => u.BookingId == id.Id).ToListAsync();
                var bcm = mapper.Map<List<Bs>>(bc);

                var br = mapper.Map<Brr>(booking);

                br.Cart_Items = bcm;

                var res = new
                {
                    Status = "Success",
                    Message = "Booking Details with Total_Amount",
                    data = br
                };
                return Ok(res);
            }
        }


        [Authorize(policy: "User")]
        [HttpPost("Create/Payment")]
        public async Task<IActionResult> Payment([FromBody] PayReq pay)
        {
            var lastdatas = await db.Payments.OrderByDescending(o => o.PaymentId)
                               .FirstOrDefaultAsync();
            long lastids = (lastdatas?.PaymentId + 1) ?? 1;


            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var booking = await db.Bookings.Where(u => u.User.UserName == username).FirstOrDefaultAsync(o => o.BookingId == pay.BookingId);

            if (booking == null)
            {
                var resn = new
                {
                    Status = "Failed",
                    Message = "bill id not found or bill id is not yours",
                    data = pay
                };
                return BadRequest(resn);

            }

            if (booking.TotalAmount != pay.Amount)
            {

                var resn = new
                {
                    Status = "Failed",
                    Message = "Payed amount is not equal to Bill Amount",
                    data = booking.TotalAmount
                };
                return BadRequest(resn);
            }

            if (booking.BookingCartTypeId == 1)
            {
                var bookcart = await db.BookingCourts.Where(o => o.BookingId == pay.BookingId).ToListAsync();
                foreach (var i in bookcart)
                {
                    i.Status = true;
                    db.SaveChanges();
                }

            }
            else
            {
                var bookcart = await db.BookingSportskits.Where(o => o.BookingId == pay.BookingId).ToListAsync();
                foreach (var i in bookcart)
                {
                    i.Status = true;
                    db.SaveChanges();
                }

            }






            var newdata = mapper.Map<Payment>(pay);

            newdata.PaymentDate = DateTime.Now.ToUniversalTime();
            newdata.PaymentId = lastids;
            newdata.PaymentStatus = true;

            booking.BookedStatus = true;

            await db.SaveChangesAsync();


            var res = new
            {
                Status = "Success",
                Message = $"Payment is Successfull for the Bill amount {pay.Amount}",
                data = booking.TotalAmount
            };
            return BadRequest(res);
        }


        [Authorize(policy: "Owner")]
        [HttpPost("Create/BookingCartType")]
        public async Task<IActionResult> cr([FromBody] TypeReq t)
        {
            var existingType = await db.BookingCartTypes
             .FirstOrDefaultAsync(o => o.BookingCartTypeName == t.BookingCartTypeName);

            if (existingType != null)
            {
                return BadRequest(new
                {
                    Status = "Failure",
                    Message = "BookingCartType name already exists",
                    Data = new
                    {
                        TypeName = t.BookingCartTypeName
                    }
                });
            }
            var lastdata = await db.BookingCartTypes.OrderByDescending(o => o.BookingCartTypeId)
                 .FirstOrDefaultAsync();
            long lastid = (lastdata?.BookingCartTypeId + 1) ?? 1;

            var newdata = new BookingCartType
            {
                BookingCartTypeId = lastid,
                BookingCartTypeName = t.BookingCartTypeName
            };

            db.BookingCartTypes.Add(newdata);
            await db.SaveChangesAsync();

            var resmodel = new
            {
                Status = "Success",
                Message = "BookingCartType is Created",
                Data = new
                {
                    BookingCartTypeId = newdata.BookingCartTypeId,
                    TypeName = newdata.BookingCartTypeName
                }
            };

            return Ok(resmodel);
        }

    }
}