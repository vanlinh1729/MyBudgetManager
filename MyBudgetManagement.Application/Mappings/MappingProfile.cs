using AutoMapper;
using MyBudgetManagement.Application.Features.AccountProfiles.Commands;
using MyBudgetManagement.Application.Features.Users.Commands;
using MyBudgetManagement.Domain.Entities;

namespace MyBudgetManagement.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateUserCommand, User>();
        CreateMap<CreateAccountProfileCommand, AccountProfile>();
    }
}