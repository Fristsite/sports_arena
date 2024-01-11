using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Play.Data;
using Play.Dto.Request;

namespace Play.Controllers
{
#nullable disable

    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        public readonly RentDbContext db;
        public readonly IMapper mapper;
        public IConfiguration _configuration;
        public LoginController(RentDbContext db, IMapper mapper, IConfiguration configuration1)
        {
            this.db = db;
            this.mapper = mapper;
            this._configuration = configuration1;
        }
        DESEncrption des = new DESEncrption();

        private string GenerateJwtToken(string username, string role, string password)
        {
            var secretKey = _configuration["JwtSettings:SecretKey"];
            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];
            var expirationDays = Convert.ToDouble(_configuration["JwtSettings:ExpirationDays"]);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.NameIdentifier,Encryptor(password,secretKey)),
            new Claim(ClaimTypes.Role, role),
            // new Claim(ClaimTypes.,id)
        };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(expirationDays),
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
        private string Encryptor(string password, string key)
        {
            var temp = password + key;
            var res = Encoding.UTF8.GetBytes(temp);
            return Convert.ToBase64String(res);
        }

        [HttpPost("Owner/Login")]
        public async Task<IActionResult> Ownerlogin([FromBody] LoginReq login)
        {

            var owner = await db.Owners.Where(a => a.IsActive != false).FirstOrDefaultAsync(o => o.UserName == login.userName);

            var secretKey = _configuration["JwtSettings:SecretKey"];
            var password = des.Encrypt(login.password, secretKey);

            if (owner == null || owner.Password != password)
            {
                var res = new
                {
                    Status = "Failed",
                    Message = "Owner Id or Password is Wrong",
                    Data = login
                };
                return NotFound(res);
            }
            var Tokens = GenerateJwtToken(login.userName, "Owner", password);

            var resmodel = new
            {
                Status = "Success",
                Message = "Login Successfull",
                Token = Tokens,
                ExpirationDays = 1

            };
            return Ok(resmodel);


        }

        [HttpPost("Employee/Login")]
        public async Task<IActionResult> EmployeeLogin([FromBody] LoginReq login)
        {

            var employee = await db.Employees.Where(a => a.IsActive != false).FirstOrDefaultAsync(o => o.UserName == login.userName);

            var secretKey = _configuration["JwtSettings:SecretKey"];

            var password = des.Encrypt(login.password, secretKey);

            if (employee == null || employee.Password != password)
            {
                var res = new
                {
                    Status = "Failed",
                    Message = "employee Id or Password is Wrong",
                    Data = login
                };
                return NotFound(res);
            }
            var Tokens = GenerateJwtToken(login.userName, "Employee", password);

            var resmodel = new
            {
                Status = "Success",
                Message = "Login Successfull",
                Token = Tokens,
                ExpirationDays = 1

            };
            return Ok(resmodel);


        }

        [HttpPost("User/Login")]
        public async Task<IActionResult> UserLogin([FromBody] LoginReq login)
        {

            var user = await db.Users.Where(a => a.IsActive != false).FirstOrDefaultAsync(o => o.UserName == login.userName);

            var secretKey = _configuration["JwtSettings:SecretKey"];

            var password = des.Encrypt(login.password, secretKey);

            if (user == null || user.Password != password)
            {
                var res = new
                {
                    Status = "Failed",
                    Message = "User Id or Password is Wrong",
                    Data = login
                };
                return NotFound(res);
            }
            var Tokens = GenerateJwtToken(login.userName, "User", password);

            var resmodel = new
            {
                Status = "Success",
                Message = "Login Successfull",
                Token = Tokens,
                ExpirationDays = 1

            };
            return Ok(resmodel);


        }


    }
}