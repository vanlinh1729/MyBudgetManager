using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;

namespace MyBudgetManagement.Application.Features.Users.Queries;

public class GetAllUserQuery : IRequest<IEnumerable<User>>
{
    internal class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, IEnumerable<User>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllUserQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<User>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var listUser = await _context.Users.ToListAsync(cancellationToken);
            return listUser;
        }
    }
}