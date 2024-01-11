using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Play.Data;
using Play.Dto.Request;
using Play.Models;

namespace Play.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        public readonly RentDbContext db;
        public readonly IMapper mapper;
        public IConfiguration _configuration;
        public EmployeeController(RentDbContext db, IMapper mapper, IConfiguration configuration1)
        {
            this.db = db;
            this.mapper = mapper;
            this._configuration = configuration1;
        }
        [Authorize(policy: "Employee")]
        [HttpPost("Add/PersonTovisit/")]
        public async Task<IActionResult> Personvisit([FromBody] Pr p)
        {
            var lastdatas = await db.PersonToPlays.OrderByDescending(o => o.PersonToId)
                               .FirstOrDefaultAsync();
            long lastids = (lastdatas?.PersonToId + 1) ?? 1;


            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var ArenaId = await db.Arenas.Where(u => u.Employee.UserName == username).Select(a => a.ArenaId).FirstOrDefaultAsync();


            var Bookings = await db.Bookings.Where(o => o.ArenaId == ArenaId && o.BookingId == p.BookingId).FirstOrDefaultAsync();
            if (Bookings == null)
            {
                var resn = new
                {
                    Status = "Failed",
                    Message = "bill id not found or bill id is not yours",
                    data = p
                };
                return BadRequest(resn);
            }

            var newData = mapper.Map<PersonToPlay>(p);
            newData.PersonToId = lastids;

            db.PersonToPlays.Add(newData);
            db.SaveChanges();

            var resd = mapper.Map<PersonToPlay>(newData);
            var res = new
            {
                Status = "Success",
                Message = "Person is added successfull",
                data = resd
            };
            return Ok(res);

        }

        // [Authorize(policy: "Employee")]
        [HttpPost("Add/PersonTovisita/")]
        public async Task<IActionResult> UploadPersonVisitExcel(IFormFile excelFile)
        {
            try
            {
                if (excelFile == null || excelFile.Length == 0)
                {
                    return BadRequest("Excel file is missing or empty.");
                }

                using (var stream = new MemoryStream())
                {
                    await excelFile.CopyToAsync(stream);

                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                        if (worksheet == null)
                        {
                            return BadRequest("Excel file doesn't contain any worksheets.");
                        }

                        var lastPersonToId = await db.PersonToPlays.OrderByDescending(o => o.PersonToId).Select(p => p.PersonToId).FirstOrDefaultAsync();
                        long lastId = lastPersonToId + 1;

                        var username = User.FindFirst(ClaimTypes.Name)?.Value;
                        var arenaId = await db.Arenas.Where(u => u.Employee.UserName == username).Select(a => a.ArenaId).FirstOrDefaultAsync();

                        var bookings = await db.Bookings.Where(o => o.ArenaId == arenaId).ToListAsync();

                        var dataRows = worksheet.Cells
                            .Select(cell => cell.Start.Row)
                            .Distinct()
                            .Skip(1);

                        foreach (var row in dataRows)
                        {
                            var p = new Pr
                            {
                                BookingId = worksheet.Cells[row, 1].GetValue<int>(),
                                Person_Name = worksheet.Cells[row, 2].GetValue<string>()

                            };

                            var booking = bookings.FirstOrDefault(b => b.BookingId == p.BookingId);
                            if (booking == null)
                            {
                                continue;
                            }

                            var newData = mapper.Map<PersonToPlay>(p);
                            newData.PersonToId = lastId;

                            await db.PersonToPlays.AddAsync(newData);
                            lastId++;
                            await db.SaveChangesAsync();
                        }

                        await db.SaveChangesAsync();

                        var response = new
                        {
                            Status = "Success",
                            Message = "Persons have been added successfully."
                        };

                        return Ok(response);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



    }
}