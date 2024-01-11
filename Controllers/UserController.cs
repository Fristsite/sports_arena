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
using Play.Dto.Response;
using Play.Models;

namespace Play.Controllers
{
#nullable disable
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {

        public readonly RentDbContext db;
        public readonly IMapper mapper;
        public IConfiguration _configuration;
        public UserController(RentDbContext db, IMapper mapper, IConfiguration configuration1)
        {
            this.db = db;
            this.mapper = mapper;
            this._configuration = configuration1;
        }

        DESEncrption des = new DESEncrption();


        [HttpPost("User/Create")]
        public async Task<IActionResult> CreateOwner([FromBody] userReq user)
        {

            var secretKey = _configuration["JwtSettings:SecretKey"];

            var lastdata = await db.Users.OrderByDescending(o => o.UserId)
                                   .FirstOrDefaultAsync();
            long lastid = (lastdata?.UserId + 1) ?? 1;

            var owne = await db.Users.FirstOrDefaultAsync(o => o.UserName == user.UserName);

            if (owne != null)
            {
                var ress = new
                {
                    Status = "Failed",
                    Message = "Username is already exists",
                    Data = user

                };
                return BadRequest(ress);
            }

            var newData = mapper.Map<User>(user);



            var encrptedPassword = des.Encrypt(user.Password, secretKey);

            newData.UserId = lastid;
            newData.CreatedAt = DateTime.Now.ToUniversalTime();
            newData.Password = encrptedPassword;
            newData.IsActive = true;

            await db.Users.AddAsync(newData);
            await db.SaveChangesAsync();
            var res = mapper.Map<UserRes>(newData);


            var resmodel = new
            {
                Status = "Success",
                Message = "User is Created",
                Owner = res
            };

            return Ok(resmodel);

        }
        [Authorize(Policy = "User")]
        [HttpPut("update/user/Details")]
        public async Task<IActionResult> update([FromBody] UserRess user)
        {
            var secretKey = _configuration["JwtSettings:SecretKey"];

            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var userId = await db.Users
                        .Where(o => o.UserName == username)
                        .Select(o => o.UserId)
                        .FirstOrDefaultAsync();



            var usernmae = await db.Users.FirstOrDefaultAsync(o => o.UserName == user.UserName);

            if (usernmae != null)
            {

                var res = new
                {
                    Status = "failed",
                    Message = "User Name is already Exists",
                    Data = user
                };
                return BadRequest(res);
            }

            var encrptedPassword = des.Encrypt(user.Password, secretKey);

            usernmae.UserName = user.UserName;
            usernmae.FullName = user.FullName;
            usernmae.Password = encrptedPassword;
            usernmae.PhoneNumber = user.PhoneNumber;







            await db.SaveChangesAsync();

            var resmodel = new
            {
                Status = "Success",
                Message = "User Successfully  updated",
                Data = user
            };
            return Ok(resmodel);

        }

        [Authorize(Policy = "User")]
        [HttpDelete("Delete/User/Details")]
        public async Task<IActionResult> Delete([FromBody] IDReq iD)
        {

            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await db.Users
                        .Where(o => o.UserName == username)

                        .FirstOrDefaultAsync();

            user.IsActive = false;

            await db.SaveChangesAsync();

            var resmodel = new
            {
                Status = "Success",
                Message = "User is Delelted Successfully",
                Data = iD
            };
            return Ok(resmodel);

        }













    }
}