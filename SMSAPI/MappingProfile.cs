using AutoMapper;
using SmsWebAPI.Dtos;
using SmsWebAPI.Entities;
namespace SmsWebAPI
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<UserForRegistrationDto, User>()
			.ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));

			CreateMap<UserForLoginDto, User>()
			.ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));

			CreateMap<UserDeleteDto, User>()
			.ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Id));

			CreateMap<UserRoleChangeDto, User>()
			.ForMember(u => u.Id, opt => opt.MapFrom(x => x.Id));

		}
	}
}
