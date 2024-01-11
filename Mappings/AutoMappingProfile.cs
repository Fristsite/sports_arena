using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Play.Dto.Request;
using Play.Dto.Response;
// using Play.Migrations;
using Play.Models;

namespace Play.Mappings
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<Owner, OwnerReq>().ReverseMap();
            CreateMap<Employee, EmployeeReq>().ReverseMap();
            CreateMap<Employee, EmployeeReqs>().ReverseMap();

            CreateMap<Arena, ArenaReq>().ReverseMap();
            CreateMap<Arena, ArenaRes>().ReverseMap();
            CreateMap<CourtDetails, CourtReq>().ReverseMap();
            CreateMap<CourtDetails, Courtres>().ReverseMap();
            CreateMap<SportsKit, KitRequest>().ReverseMap();
            CreateMap<SportsKit, KitRes>().ReverseMap();

            CreateMap<User, userReq>().ReverseMap();
            CreateMap<User, UserRes>().ReverseMap();
            CreateMap<User, UserRess>().ReverseMap();
            CreateMap<Bc, Bc>().ReverseMap();
            CreateMap<BookingCourt,Bc>().ReverseMap();
            CreateMap<Bokreq, Booking>().ReverseMap();
            CreateMap<Br, Booking>().ReverseMap();
            CreateMap<Payment, PayReq>().ReverseMap();
            CreateMap<RatingReq, Ratings>().ReverseMap();
            CreateMap<RatingReqs, Ratings>().ReverseMap();
            CreateMap<PersonToPlay,Pr>().ReverseMap();
            CreateMap<Game,GameReq>().ReverseMap();
            CreateMap<Address,AddressReq>().ReverseMap();
            CreateMap<Addresss,Address>().ReverseMap();
            CreateMap<BookingSportskit, Bc>().ReverseMap();

            CreateMap<BookingSportskit,Bs>().ReverseMap();
            CreateMap<Booking,Brr>().ReverseMap();
        }
    }
}