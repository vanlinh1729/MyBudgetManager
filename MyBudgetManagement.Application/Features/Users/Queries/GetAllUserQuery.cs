using MediatR;
using MyBudgetManagement.Domain.Entities;

namespace MyBudgetManagement.Application.Features.Users.Queries;

public class GetAllUserQuery : IRequest<IEnumerable<User>>
{
    internal class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, IEnumerable<User>>
    {
        public async Task<IEnumerable<User>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var listUser = new List<User>();
            for (int i = 0; i < 100; i++)
            {
                listUser.Add(new User{FirstName = "tést" + i});
            }

            return listUser;
        }
    }
}