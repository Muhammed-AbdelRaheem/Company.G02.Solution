using AutoMapper;
using Company.G02.DAL.Models;
using Company.G02.PL.ViewModels.Users;

namespace Company.G02.PL.Mapping.Users
{
    public class UserProfile:Profile
    {

        public UserProfile ()
        {
            CreateMap<ApplicationUser, UserViewModel>().ReverseMap();
        }
    }
}
