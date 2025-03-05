using MediatR;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Enums;
using MyBudgetManagement.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyBudgetManagement.Application.Features.Categories.Commands
{
    public class CreateCategoryCommand : IRequest<ApiResponse<string>>
    {
        public string Name { get; set; }
        public decimal Budget { get; set; }
        public CategoryType CategoryType { get; set; }
        public Guid UserBalanceId { get; set; }
    }

    internal class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, ApiResponse<string>>
    {
        private readonly ICategoryRepositoryAsync _categoryRepository;
        private readonly IUserRepositoryAsync _userRepository;

        public CreateCategoryCommandHandler(ICategoryRepositoryAsync categoryRepository, IUserRepositoryAsync userRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<ApiResponse<string>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var user = await _userRepository.GetUserByUserBalanceAsync(request.UserBalanceId);
            if (user == null)
            {
                return new ApiResponse<string>("User not found.");
            }

            var category = new Category
            {
                Name = request.Name,
                Budget = request.Budget,
                CategoryType = request.CategoryType,
                Created = DateTime.UtcNow,
                CreatedBy = user.Id.ToString(),
                LastModifiedBy = user.Id.ToString(),
                UserBalanceId = request.UserBalanceId
            };

            await _categoryRepository.AddAsync(category);

            return new ApiResponse<string>("Category created successfully.");
        }
    }
}