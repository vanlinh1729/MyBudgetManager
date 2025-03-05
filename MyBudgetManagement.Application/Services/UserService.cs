using AutoMapper;
using MyBudgetManagement.Application.DTOs;
using MyBudgetManagement.Application.Exceptions;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Application.Helpers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Enums;
using MyBudgetManagement.Domain.Interfaces;
using System.Text.RegularExpressions;
using System.Transactions;

namespace MyBudgetManagement.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepositoryAsync _userRepository;
    private readonly IUserRoleRepositoryAsync _userRoleRepository;
    private readonly IRoleRepositoryAsync _roleRepository;
    private readonly IAccountProfileRepositoryAsync _accountProfileRepository;
    private readonly IUserBalanceRepositoryAsync _userBalanceRepository;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;

    private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    public UserService(IUserRepositoryAsync userRepository, IUserRoleRepositoryAsync userRoleRepository, IRoleRepositoryAsync roleRepository, IAccountProfileRepositoryAsync accountProfileRepository, IUserBalanceRepositoryAsync userBalanceRepository, IMapper mapper, IEmailService emailService)
    {
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
        _roleRepository = roleRepository;
        _accountProfileRepository = accountProfileRepository;
        _userBalanceRepository = userBalanceRepository;
        _mapper = mapper;
        _emailService = emailService;
    }
    public async Task<UserDto> CreateUserAsync(CreateUserDto request, CancellationToken cancellationToken = default)
    {
        // Validate email format
        if (!EmailRegex.IsMatch(request.Email))
            throw new ApiException("Invalid email format.");

        // Check password is not empty
        if (string.IsNullOrWhiteSpace(request.Password))
            throw new ApiException("Password cannot be empty.");

        // Check if email already exists
        if (await _userRepository.GetUserByEmailAsync(request.Email) != null)
            throw new ApiException("Email already exists, please try another email.");

        // Get "User" role
        var role = await _roleRepository.GetRoleByRoleNameAsync("User")
            ?? throw new ApiException("Default role 'User' not found.");

        try
        {
            // Create new user
            var user = _mapper.Map<User>(request);
            user.PasswordHash = BCryptHelper.HashPassword(request.Password);
            user.Id = Guid.NewGuid();
            user.CreatedBy = user.Id.ToString();
            user.Created = DateTime.UtcNow;
            user.LastModifiedBy = user.Id.ToString();
            user.LastModified = DateTime.UtcNow;

            await _userRepository.AddAsync(user);

            // Assign user to role
            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = role.Id,
                CreatedBy = user.Id.ToString(),
                Created = DateTime.UtcNow,
                LastModifiedBy = user.Id.ToString(),
                LastModified = DateTime.UtcNow
            };
            await _userRoleRepository.AddAsync(userRole);

            // Create AccountProfile
            var accountProfile = new AccountProfile
            {
                UserId = user.Id,
                Currency = Currencies.VND,
                Gender = Gender.Other,
                CreatedBy = user.Id.ToString(),
                Created = DateTime.UtcNow,
                LastModifiedBy = user.Id.ToString(),
                LastModified = DateTime.UtcNow
            };
            await _accountProfileRepository.AddAsync(accountProfile);

            // Create UserBalance
            var userBalance = new UserBalance
            {
                UserId = user.Id,
                CreatedBy = user.Id.ToString(),
                Created = DateTime.UtcNow,
                LastModifiedBy = user.Id.ToString(),
                LastModified = DateTime.UtcNow
            };
            await _userBalanceRepository.AddAsync(userBalance);
            var accountProfileDto = _mapper.Map<AccountProfileDto>(accountProfile);
            var userRoles = await _userRoleRepository.GetUserRolesByUserIdAsync(user.Id);
            var userBalanceDto =  _mapper.Map<UserBalanceDto>(userBalance);
            var listUserRoles = _mapper.Map<List<UserRoleDto>>(userRoles);
            var userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                AccountProfile = accountProfileDto,
                Created = user.Created,
                CreatedBy = user.CreatedBy,
                UserBalance = userBalanceDto,
                UserRoles = listUserRoles
            };
            // Gửi email chào mừng sau khi đăng ký
            string subject = "Welcome to MyBudgetManagement!";
            string body = $@"
        <!DOCTYPE html>
        <html lang='en'>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <title>Registration Successful</title>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #f4f4f4;
                    margin: 0;
                    padding: 0;
                }}
                .container {{
                    width: 100%;
                    max-width: 600px;
                    margin: 0 auto;
                    background-color: #ffffff;
                    padding: 20px;
                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                }}
                .header {{
                    text-align: center;
                    padding: 10px 0;
                    background-color: #4CAF50;
                    color: white;
                }}
                .content {{
                    padding: 20px;
                }}
                .footer {{
                    text-align: center;
                    padding: 10px 0;
                    background-color: #f4f4f4;
                    color: #666666;
                    font-size: 12px;
                }}
                h1 {{
                    color: #333333;
                }}
                p {{
                    color: #666666;
                    line-height: 1.5;
                }}
                .button {{
                    display: inline-block;
                    padding: 10px 20px;
                    margin: 20px 0;
                    font-size: 16px;
                    color: white;
                    background-color: #4CAF50;
                    text-decoration: none;
                    border-radius: 5px;
                }}
                .button:hover {{
                    background-color: #45a049;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='header'>
                    <h1>Welcome to Our Service!</h1>
                </div>
                <div class='content'>
                    <h1>Hi, {user.FirstName}!</h1>
                    <p>We are excited to have you on board. Your account has been successfully created. You can now start using our service to enjoy the features and benefits we offer.</p>
                    <p>If you have any questions, feel free to contact our support team.</p>
                    <a href='https://mybudgetmanagement.nguyenvanlinh.io.vn/login' class='button'>Log in to your account</a>
                </div>
                <div class='footer'>
                    <p>&copy; {DateTime.Now.Year} My BudgetManagement. All rights reserved.</p>
                    <p>Hanoi, Vietnam</p>
                </div>
            </div>
        </body>
        </html>";
            var emailBody = $$"""
                  <!DOCTYPE html>
                  <html xmlns:v="urn:schemas-microsoft-com:vml" xmlns:o="urn:schemas-microsoft-com:office:office" lang="en">
                  
                  <head>
                      <title>Registration Success</title>
                      <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
                      <meta name="viewport" content="width=device-width, initial-scale=1.0">
                      <link href="https://fonts.googleapis.com/css2?family=Abril+Fatface:wght@100;400;700" rel="stylesheet" type="text/css">
                      <style>
                          * {
                              box-sizing: border-box;
                          }
                  
                          body {
                              margin: 0;
                              padding: 0;
                              background-color: #f8f9fa; /* Màu nền tổng thể */
                              font-family: Arial, sans-serif;
                          }
                  
                          .container {
                              width: 100%;
                              max-width: 600px;
                              margin: 20px auto;
                              background-color: #ffffff; /* Màu nền thẻ */
                              border-radius: 8px;
                              box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
                          }
                  
                          .header {
                              background-color: #7747FF; /* Màu nền đầu thẻ */
                              color: #ffffff;
                              text-align: center;
                              padding: 20px;
                          }
                  
                          .header h1 {
                              margin: 0;
                              font-family: 'Abril Fatface', serif;
                              font-size: 36px;
                          }
                  
                          .image-block {
                              text-align: center;
                              padding: 20px;
                          }
                  
                          .image-block img {
                              width: 100%;
                              height: auto;
                              border-radius: 8px;
                          }
                  
                          .content {
                              padding: 20px;
                              color: #101112;
                          }
                  
                          .content p {
                              margin: 0 0 16px;
                              line-height: 1.5;
                          }
                  
                          .button {
                              display: inline-block;
                              background-color: #7747FF;
                              color: #ffffff;
                              padding: 10px 20px;
                              border-radius: 4px;
                              text-decoration: none;
                              font-weight: bold;
                              text-align: center;
                              margin: 20px auto; /* Căn giữa */
                          }
                  
                          .social-links {
                              text-align: center;
                              margin-top: 20px;
                          }
                  
                          .social-links img {
                              width: 32px;
                              height: auto;
                              margin: 0 5px;
                          }
                  
                          .footer {
                              text-align: center;
                              padding: 10px;
                              background-color: #f1f1f1;
                              font-size: 14px;
                              color: #101112;
                          }
                      </style>
                  </head>
                  
                  <body>
                      <div class="container">
                          <div class="header">
                              <h1>Welcome to MyBudgetManagement</h1>
                          </div>
                          <div class="image-block">
                              <img src="https://d15k2d11r6t6rl.cloudfront.net/pub/bfra/4qcmm84c/jvb/idn/5jx/1.png" alt="Budget Management">
                          </div>
                          <div class="content">
                              <p><strong>Hi User,</strong></p>
                              <p>Congratulations on starting your journey with MyBudgetManagement.</p>
                              <p>We are excited to have you on board. Your account has been successfully created. You can now start using our service to enjoy the features and benefits we offer.</p>
                              <p>If you have any questions, feel free to contact our support team.</p>
                              <div style="text-align: center;"> <!-- Thêm div này để căn giữa nút -->
                                  <a href="https://mybudgetmanagement.nguyenvanlinh.io.vn/login" class="button">Explore Now</a>
                              </div>
                          </div>
                          <div class="social-links">
                              <a href="https://www.facebook.com/nvl.1712" target="_blank">
                                  <img src="https://app-rsrc.getbee.io/public/resources/social-networks-icon-sets/t-only-logo-dark-gray/facebook@2x.png" alt="Facebook">
                              </a>
                              <a href="mailto:vanlinhnguyen1729@gmail.com?subject=Hi, I need your support!" target="_blank">
                                  <img src="https://app-rsrc.getbee.io/public/resources/social-networks-icon-sets/t-only-logo-dark-gray/mail@2x.png" alt="E-Mail">
                              </a>
                          </div>
                          <div class="footer">
                              <p>© 2025 MyBudgetManagement. All rights reserved.</p>
                              <p>Hanoi, Vietnam</p>
                          </div>
                      </div>
                  </body>
                  
                  </html>
                  
                  """;


            await _emailService.SendEmailAsync(user.Email, subject, emailBody);

            
            return userDto;
        }
        catch (Exception e)
        {
            throw new ApiException("Error creating user."+ e.Message);
        }
    }

    public async Task<UserDto> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            return _mapper.Map<UserDto>(user);
        }
        catch (Exception e)
        {
            throw new ApiException("Error getting user by Email: "+ e.Message);
        }
       
    }
}
