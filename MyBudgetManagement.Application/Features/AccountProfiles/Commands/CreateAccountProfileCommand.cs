using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Exceptions;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Application.Features.AccountProfiles.Commands;

public class CreateAccountProfileCommand : IRequest<ApiResponse<Guid>>
{
    public Guid UserId { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Avatar { get; set; }
    public string? Address { get; set; }
    public Gender Gender { get; set; }
    public Currencies Currency { get; set; } = Currencies.VND;

    internal class CreateAccountProfileCommandHandler : IRequestHandler<CreateAccountProfileCommand,ApiResponse<Guid>>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public CreateAccountProfileCommandHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ApiResponse<Guid>> Handle(CreateAccountProfileCommand request, CancellationToken cancellationToken)
        {
            // Kiểm tra người dùng có tồn tại không
            var result = await _context.Users.Where(x => x.Id == request.UserId).FirstOrDefaultAsync();
            if (result == null)
            {
                throw new ApiException("User not found.");
            }
            var accountProfile = _mapper.Map<AccountProfile>(request);
            accountProfile.User = result;
            accountProfile.CreatedBy = request.UserId.ToString();
            accountProfile.Created = DateTime.Now;
            accountProfile.LastModifiedBy = request.UserId.ToString();
            accountProfile.LastModified = DateTime.Now;
            await _context.AccountProfiles.AddAsync(accountProfile, cancellationToken);
            await _context.SaveChangesAsync(); 
            return new ApiResponse<Guid>(accountProfile.Id, "AccountProfile created successfully");
        }
    }
}