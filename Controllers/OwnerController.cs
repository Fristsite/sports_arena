using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Play.Data;
using Play.Dto;
using Play.Dto.Request;
using Play.Dto.Response;
using Play.Models;

namespace Play.Controllers
{
#nullable disable
    [ApiController]
    [Route("api/[controller]")]
    public class OwnerController : ControllerBase
    {
        public readonly RentDbContext db;
        public readonly IMapper mapper;
        public IConfiguration _configuration;
        public OwnerController(RentDbContext db, IMapper mapper, IConfiguration configuration1)
        {
            this.db = db;
            this.mapper = mapper;
            this._configuration = configuration1;
        }

        DESEncrption des = new DESEncrption();



        [HttpPost("Owner/Create")]
        public async Task<IActionResult> CreateOwner([FromBody] OwnerReq owner)
        {

            var secretKey = _configuration["JwtSettings:SecretKey"];

            var lastdata = await db.Owners.OrderByDescending(o => o.OwnerId)
                                   .FirstOrDefaultAsync();
            long lastid = (lastdata?.OwnerId + 1) ?? 1;

            var owne = await db.Owners.FirstOrDefaultAsync(o => o.UserName == owner.UserName);

            if (owne != null)
            {
                var res = new
                {
                    Status = "Failed",
                    Message = "Username is already exists",
                    Data = owner

                };
                return BadRequest(res);
            }

            var newData = mapper.Map<Owner>(owner);

            var encrptedPassword = des.Encrypt(owner.Password, secretKey);

            newData.OwnerId = lastid;
            newData.CreatedAt = DateTime.Now.ToUniversalTime();
            newData.Password = encrptedPassword;
            newData.IsActive = true;

            await db.Owners.AddAsync(newData);
            await db.SaveChangesAsync();


            var resmodel = new OwnerRes
            {
                Status = "Success",
                Message = "Owner is Created",
                Owner = newData
            };

            return Ok(resmodel);

        }
        [Authorize(Roles = "Owner")]
        [HttpPut("Owner/Update/")]
        public async Task<IActionResult> UpdateOwner([FromBody] OwnerReq owner)
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var secretKey = _configuration["JwtSettings:SecretKey"];

            var own = await db.Owners.FirstOrDefaultAsync(o => o.UserName == userName);

            var ownera = await db.Owners.Where(a => a.IsActive != false).FirstOrDefaultAsync(o => o.OwnerId == own.OwnerId);

            var encrptedPassword = des.Encrypt(owner.Password, secretKey);

            ownera.OwnerName = owner.OwnerName;
            ownera.Password = encrptedPassword;
            ownera.PhoneNumber = owner.PhoneNumber;


            var owne = await db.Owners.FirstOrDefaultAsync(o => o.UserName == owner.UserName);

            if (owne != null)
            {
                var res = new
                {
                    Status = "Failed",
                    Message = "Username is already exists",
                    Data = owner

                };
                return BadRequest(res);
            }
            ownera.UserName = owner.UserName;
            db.SaveChanges();

            var resmodel = new
            {
                Status = "Success",
                Message = "Owner is updated successfully",
                Id = own.OwnerId,
                Owner = owner
            };

            return Ok(resmodel);


        }
        [Authorize(Roles = "Owner")]

        [HttpDelete("Owner/Delete/")]
        public async Task<IActionResult> DeleteOwner()
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var own = await db.Owners.FirstOrDefaultAsync(o => o.UserName == userName);
            if (own == null)
            {
                var res = new
                {
                    Status = "Failed",
                    Message = "Owner not found"


                };

                return NotFound(res);
            }
            var ownera = await db.Owners.Where(a => a.IsActive != false).FirstOrDefaultAsync(o => o.OwnerId == own.OwnerId);
            ownera.IsActive = false;
            db.SaveChanges();

            var ress = new
            {
                Status = "Success",
                Message = "Owner is Delelted successfully",
                Id = own.OwnerId

            };

            return Ok(ress);

        }
        [Authorize(Roles = "Owner")]
        [HttpPost("Employee/Create")]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeReq emp)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var secretKey = _configuration["JwtSettings:SecretKey"];

            var lastdata = await db.Employees.OrderByDescending(o => o.EmployeeId)
                                   .FirstOrDefaultAsync();
            long lastid = (lastdata?.EmployeeId + 1) ?? 1;

            var ownerId = await db.Owners
                        .Where(o => o.UserName == username)
                        .Select(a => a.OwnerId)
                        .FirstOrDefaultAsync();
            var date = DateTime.Today;


            var owne = await db.Employees.FirstOrDefaultAsync(o => o.UserName == emp.UserName);
            if (owne != null)
            {
                var res = new
                {
                    Status = "Failed",
                    Message = "Username is already exists",
                    Data = emp

                };
                return BadRequest(res);
            }

            var arena = db.Arenas.Where(o=>o.OwnerId==ownerId).FirstOrDefault(o => o.ArenaId == emp.ArenaId);
            if (arena == null)
            {
                var res = new
                {
                    Status = "Failed",
                    Message = "Arena id is not yours",
                    Data = emp

                };
                return BadRequest(res);
            }

            var newData = mapper.Map<Employee>(emp);
            
            var encrptedPassword = des.Encrypt(emp.Password, secretKey);
            // var date = DateTime.Now;
            newData.EmployeeId = lastid;
            newData.DateofJoined = DateOnly.FromDateTime(date);
            newData.Password = encrptedPassword;
            newData.IsActive = true;

            await db.Employees.AddAsync(newData);
            await db.SaveChangesAsync();

            var resmodel1 = mapper.Map<EmployeeReqs>(newData);

            var resmodel = new
            {
                Status = "Success",
                Message = "Employee is Created",
                Employee = resmodel1
            };

          



            return Ok(resmodel);

        }



    }
}