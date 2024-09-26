using AutoMapper;
using Company.G02.PL.ViewModels.Roles;
using Microsoft.AspNetCore.Identity;

namespace Company.G02.PL.Mapping.Roles
{
    public class RoleProfile : Profile
    {

        public RoleProfile()
        {
            CreateMap<IdentityRole, RoleViewModel>().ForMember(D=>D.RoleName,O=>O.MapFrom(S=>S.Name)).ReverseMap();

        }


    }
}
