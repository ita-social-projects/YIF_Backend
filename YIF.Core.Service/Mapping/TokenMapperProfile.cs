using AutoMapper;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.DtoModels;

namespace YIF.Core.Service.Mapping
{
    public class TokenMapperProfile : Profile
    {
        public TokenMapperProfile()
        {
            AllowNullCollections = true;
            CreateMap<Token, TokenDTO>().ReverseMap();
        }
    }
}
