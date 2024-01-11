using System.Security.Claims;
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
    [ApiController]
    [Route("api/[controller]")]
    public class ArenaController : ControllerBase
    {
#nullable disable
        public readonly RentDbContext db;
        public readonly IMapper mapper;
        public IConfiguration _configuration;
        public ArenaController(RentDbContext db, IMapper mapper, IConfiguration configuration1)
        {
            this.db = db;
            this.mapper = mapper;
            this._configuration = configuration1;
            
        }
        [Authorize(Policy = "Owner")]
        [HttpPost("Add/Arena/Details")]
        public async Task<IActionResult> Create([FromBody] ArenaReq arenaReq)
        {


            var lastId = await db.Arenas.MaxAsync(o => (long?)o.ArenaId) ?? 0;
            long nextId = lastId + 1;

            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var ownerId = await db.Owners.Where(o=>o.UserName==username).Select(a=>a.OwnerId).FirstOrDefaultAsync();

            var arena = await db.Arenas.FirstOrDefaultAsync(o => o.ArenaName == arenaReq.ArenaName);

            if (arena != null)
            {

                var res = new
                {
                    Status = "failed",
                    Message = "Arena Name is already Exists",
                    Data = arenaReq
                };
                return BadRequest(res);
            }

            var newData = mapper.Map<Arena>(arenaReq);


            newData.DateofAdded = DateOnly.FromDateTime(DateTime.Now);
            newData.OwnerId = ownerId;
            newData.ArenaId = nextId;
            newData.IsActive = true;

            var resData = mapper.Map<ArenaRes>(newData);

            await db.Arenas.AddAsync(newData);
            await db.SaveChangesAsync();

            var resmodel = new
            {
                Status = "Success",
                Message = "Successfully Created Arena",
                Data = resData
            };
            return Ok(resmodel);


        }
        [Authorize(Policy = "Owner")]
        [HttpPut("update/Arena/Details")]
        public async Task<IActionResult> update([FromBody] ArenaRes arenaReq)
        {

            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var ownerId = await db.Owners
                        .Where(o => o.UserName == username)
                        .Select(o => o.OwnerId)
                        .FirstOrDefaultAsync();

            var arena1 = await db.Arenas.Where(o => o.OwnerId == ownerId).Select(a => a.ArenaId).ToListAsync();
            int a = 0;
            foreach (long i in arena1)
            {
                if (i == arenaReq.ArenaId)
                {
                    a = 1;
                }
            }
            if (a == 0)
            {
                var res = new
                {
                    Status = "failed",
                    Message = "Arena id is not exist or your's",
                    Data = arenaReq
                };
                return BadRequest(res);

            }


            var arena = await db.Arenas.FirstOrDefaultAsync(o => o.ArenaName == arenaReq.ArenaName);

            if (arena != null)
            {

                var res = new
                {
                    Status = "failed",
                    Message = "Arena Name is already Exists",
                    Data = arenaReq
                };
                return BadRequest(res);
            }

            var ar = await db.Arenas.FirstOrDefaultAsync(o => o.ArenaId == arenaReq.ArenaId);

            ar.AddressId = arenaReq.AddressId;
            ar.ArenaName = arenaReq.ArenaName;




            await db.SaveChangesAsync();

            var resmodel = new
            {
                Status = "Success",
                Message = "Arena Successfully  updated",
                Data = arenaReq
            };
            return Ok(resmodel);

        }

        [Authorize(Policy = "Owner")]
        [HttpDelete("Delete/Arena/Details")]
        public async Task<IActionResult> Delete([FromBody] IDReq iD)
        {

            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var ownerId = await db.Owners
                        .Where(o => o.UserName == username)
                        .Select(o => o.OwnerId)
                        .FirstOrDefaultAsync();

            var arena1 = await db.Arenas.Where(o => o.OwnerId == ownerId).Select(a => a.ArenaId).ToListAsync();
            int a = 0;
            foreach (long i in arena1)
            {
                if (i == iD.Id)
                {
                    a = 1;
                }
            }
            if (a == 0 || arena1 == null)
            {
                var res = new
                {
                    Status = "failed",
                    Message = "Arena id is not exist or your's",
                    Data = iD
                };
                return BadRequest(res);

            }



            var ar = await db.Arenas.FirstOrDefaultAsync(o => o.ArenaId == iD.Id);

            ar.IsActive = false;

            await db.SaveChangesAsync();

            var resmodel = new
            {
                Status = "Success",
                Message = "Arena is Delelted Successfully",
                Data = iD
            };
            return Ok(resmodel);

        }
        [Authorize(Policy = "Owner")]
        [HttpPost("Add/Arena/Court/Details")]
        public async Task<IActionResult> CreateCOurt([FromBody] CourtReq courtReq)
        {


            var lastId = await db.CourtDetails.MaxAsync(o => (long?)o.ItemId) ?? 0;
            long nextId = lastId + 1;

            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var ownerId = await db.Owners
                        .Where(o => o.UserName == username)
                        .Select(o => o.OwnerId)
                        .FirstOrDefaultAsync();

            var arena1 = await db.Arenas.Where(o => o.OwnerId == ownerId).Select(a => a.ArenaId).ToListAsync();
            int a = 0;
            foreach (long i in arena1)
            {
                if (i == courtReq.ArenaId)
                {
                    a = 1;
                }
            }
            if (a == 0)
            {
                var res = new
                {
                    Status = "failed",
                    Message = "Arena id is not exist or your's",
                    Data = courtReq
                };
                return BadRequest(res);

            }

            var arena = await db.CourtDetails.Where(a => a.ArenaId == courtReq.ArenaId).FirstOrDefaultAsync(o => o.CourtName == courtReq.CourtName);

            if (arena != null)
            {

                var res = new
                {
                    Status = "failed",
                    Message = "Court Name is already Exists",
                    Data = courtReq
                };
                return BadRequest(res);
            }

            var newData = mapper.Map<CourtDetails>(courtReq);
            

            newData.ItemId = nextId;
            newData.IsActive = true;


            await db.CourtDetails.AddAsync(newData);
            await db.SaveChangesAsync();
            var resData = mapper.Map<Courtres>(newData);

            var resmodel = new
            {
                Status = "Success",
                Message = "Successfully Created courtDetails",
                Data = resData
            };
            return Ok(resmodel);


        }
        [Authorize(Policy = "Owner")]
        [HttpPut("update/Arena/Court/Details")]
        public async Task<IActionResult> updateCOurt([FromBody] Courtup up)
        {

            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var ownerId = await db.Owners
                        .Where(o => o.UserName == username)
                        .Select(o => o.OwnerId)
                        .FirstOrDefaultAsync();




            var arena1 = await db.Arenas.Where(o => o.OwnerId == ownerId).Include(c => c.CourtDetails).Select(a =>
                a.CourtDetails.Select(i => i.ItemId)
            ).ToListAsync();




            int a = 0;
            foreach (long i in arena1[0])
            {
                if (i == up.ItemId)
                {
                    a = 1;
                }
            }
            if (a == 0)
            {
                var res = new
                {
                    Status = "failed",
                    Message = "Court id is not exist or your's",
                    Data = up
                };
                return BadRequest(res);

            }

            var court = await db.CourtDetails
                                  .FirstOrDefaultAsync(o => o.ItemId == up.ItemId);

            var arena = db.CourtDetails.Where(n => n.ArenaId == court.ArenaId).FirstOrDefault(o => o.CourtName == up.CourtName);

            if (arena != null)
            {

                var res = new
                {
                    Status = "failed",
                    Message = "Court Name is already Exists",
                    Data = up
                };
                return BadRequest(res);
            }

            court.CourtDiscription = up.CourtDiscription;
            court.CourtName = up.CourtName;
            court.Price = up.Price;


            await db.SaveChangesAsync();

            var resmodel = new
            {
                Status = "Success",
                Message = $"Successfully updated courtDetails",
                Data = up
            };
            return Ok(resmodel);


        }

        [Authorize(Policy = "Owner")]
        [HttpDelete("Delete/Arena/Court/Details")]
        public async Task<IActionResult> Deletecout([FromBody] IDReq iD)
        {

            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var ownerId = await db.Owners
                        .Where(o => o.UserName == username)
                        .Select(o => o.OwnerId)
                        .FirstOrDefaultAsync();

            var court = await db.CourtDetails.Where(o => o.ItemId == iD.Id)
                                .FirstOrDefaultAsync();


            var arena1 = await db.Arenas.Where(o => o.OwnerId == ownerId).Select(a => a.ArenaId).ToListAsync();
            int a = 0;
            foreach (long i in arena1)
            {
                if (i == court.ArenaId)
                {
                    a = 1;
                }
            }
            if (a == 0 || court == null)
            {
                var res = new
                {
                    Status = "failed",
                    Message = "Arena id is not exist or your's",
                    Data = iD.Id
                };
                return BadRequest(res);

            }




            court.IsActive = false;

            await db.SaveChangesAsync();

            var resmodel = new
            {
                Status = "Success",
                Message = "Court is Delelted Successfully",
                Data = iD.Id
            };
            return Ok(resmodel);

        }

        [Authorize(Policy = "Owner")]
        [HttpPost("Add/Arena/Sportsket/Details")]
        public async Task<IActionResult> Createsportskit([FromBody] KitRequest kitReq)
        {


            var lastdatas = await db.SportsKits.OrderByDescending(o => o.ItemId)
                               .FirstOrDefaultAsync();
            long lastids = (lastdatas?.ItemId + 1) ?? 1;

            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var ownerId = await db.Owners
                        .Where(o => o.UserName == username)
                        .Select(o => o.OwnerId)
                        .FirstOrDefaultAsync();

            var arena1 = await db.Arenas.Where(o => o.OwnerId == ownerId).Select(a => a.ArenaId).ToListAsync();
            int a = 0;
            foreach (long i in arena1)
            {
                if (i == kitReq.ArenaId)
                {
                    a = 1;
                }
            }
            if (a == 0)
            {
                var res = new
                {
                    Status = "failed",
                    Message = "Arena id is not exist or your's",
                    Data = kitReq
                };
                return BadRequest(res);

            }


            var newData = mapper.Map<SportsKit>(kitReq);
            var resData = mapper.Map<KitRes>(newData);

            newData.ItemId = lastids;
            newData.IsActive = true;


            await db.SportsKits.AddAsync(newData);
            await db.SaveChangesAsync();

            var resmodel = new
            {
                Status = "Success",
                Message = "Successfully Created Sportkits details",
                Data = resData
            };
            return Ok(resmodel);


        }
        [Authorize(Policy = "Owner")]
        [HttpPut("Update/Arena/sportkit/Details")]
        public async Task<IActionResult> updatesportkit([FromBody] KitRes up)
        {

            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var ownerId = await db.Owners
                        .Where(o => o.UserName == username)
                        .Select(o => o.OwnerId)
                        .FirstOrDefaultAsync();




            var arena1 = await db.Arenas.Where(o => o.OwnerId == ownerId).Include(c => c.SportsKits).Select(a =>
                a.SportsKits.Select(i => i.ItemId)
            ).ToListAsync();




            int a = 0;
            foreach (long i in arena1[0])
            {
                if (i == up.ItemId)
                {
                    a = 1;
                }
            }
            if (a == 0)
            {
                var res = new
                {
                    Status = "failed",
                    Message = "sportkit id is not exist or your's",
                    Data = up
                };
                return BadRequest(res);

            }

            var kit = await db.SportsKits
                                  .FirstOrDefaultAsync(o => o.ItemId == up.ItemId);

            kit.SportsKitDescription = up.SportsKitDescription;
            kit.PricePerHour = up.PricePerHour;
            kit.Count = up.Count;




            await db.SaveChangesAsync();

            var resmodel = new
            {
                Status = "Success",
                Message = $"Successfully updated Kit Details",
                Data = up
            };
            return Ok(resmodel);

            // return Ok();


        }
        [Authorize(Policy = "Owner")]
        [HttpDelete("Delete/Arena/SportKit/Details")]
        public async Task<IActionResult> DeleteKit([FromBody] IDReq iD)
        {

            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var ownerId = await db.Owners
                        .Where(o => o.UserName == username)
                        .Select(o => o.OwnerId)
                        .FirstOrDefaultAsync();

            var sportsKit = await db.SportsKits.Where(o => o.ItemId == iD.Id)
                    .FirstOrDefaultAsync();

            var arena1 = await db.Arenas.Where(o => o.OwnerId == ownerId).Select(a => a.ArenaId).ToListAsync();
            int a = 0;
            foreach (long i in arena1)
            {
                if (i == sportsKit.ArenaId)
                {
                    a = 1;
                }
            }
            if (a == 0 || sportsKit == null)
            {
                var res = new
                {
                    Status = "failed",
                    Message = "kit id is not exist or your's",
                    Data = iD.Id
                };
                return BadRequest(res);

            }



            sportsKit.IsActive = false;

            await db.SaveChangesAsync();

            var resmodel = new
            {
                Status = "Success",
                Message = "Arena is Delelted Successfully",
                Data = iD.Id
            };
            return Ok(resmodel);

        }

        // [Authorize(policy: "Owner")]
        [HttpPost("Create/Game")]
        public async Task<IActionResult> createGame([FromBody] GameReq g)
        {
            // var lastdatas = await db.Games.OrderByDescending(o => o.GameId)
            //                                .FirstOrDefaultAsync();
            // long lastids = (lastdatas?.GameId + 1) ?? 1;

            var res = new Game
            {
            //     GameId = lastids,
                GameName = g.GameName
            };
            db.Games.Add(res);
            db.SaveChanges();

            var resmodel = new
            {
                Status = "Success",
                Message = "Game is added Successfully",
                Data = res
            };
            return Ok(resmodel);




        }




        [HttpPost("Add/Arena/Address/Details")]
        public async Task<IActionResult> CreateAddress([FromBody] AddressReq a)
        {


            var lastdatas = await db.Addresses.OrderByDescending(o => o.AddressId)
                               .FirstOrDefaultAsync();
            long lastids = (lastdatas?.AddressId + 1) ?? 10000;






            var newData = mapper.Map<Address>(a);
            var resData = mapper.Map<Addresss>(newData);

            newData.AddressId = lastids;
            // newData.IsActive = true;


            await db.Addresses.AddAsync(newData);
            await db.SaveChangesAsync();

            var resmodel = new
            {
                Status = "Success",
                Message = "Successfully Created Sportkits details",
                Data = resData
            };
            return Ok(resmodel);


        }























    }
}
