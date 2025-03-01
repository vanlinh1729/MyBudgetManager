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
	<title></title>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
	<meta name="viewport" content="width=device-width, initial-scale=1.0"><!--[if mso]><xml><o:OfficeDocumentSettings><o:PixelsPerInch>96</o:PixelsPerInch><o:AllowPNG/></o:OfficeDocumentSettings></xml><![endif]--><!--[if !mso]><!-->
	<link href="https://fonts.googleapis.com/css2?family=Abril+Fatface:wght@100;200;300;400;500;600;700;800;900" rel="stylesheet" type="text/css"><!--<![endif]-->
	<style>
		* {
			box-sizing: border-box;
		}

		body {
			margin: 0;
			padding: 0;
		}

		a[x-apple-data-detectors] {
			color: inherit !important;
			text-decoration: inherit !important;
		}

		#MessageViewBody a {
			color: inherit;
			text-decoration: none;
		}

		p {
			line-height: inherit
		}

		.desktop_hide,
		.desktop_hide table {
			mso-hide: all;
			display: none;
			max-height: 0px;
			overflow: hidden;
		}

		.image_block img+div {
			display: none;
		}

		sup,
		sub {
			font-size: 75%;
			line-height: 0;
		}

		@media (max-width:545px) {
			.social_block.desktop_hide .social-table {
				display: inline-block !important;
			}

			.image_block div.fullWidth {
				max-width: 100% !important;
			}

			.mobile_hide {
				display: none;
			}

			.row-content {
				width: 100% !important;
			}

			.stack .column {
				width: 100%;
				display: block;
			}

			.mobile_hide {
				min-height: 0;
				max-height: 0;
				max-width: 0;
				overflow: hidden;
				font-size: 0px;
			}

			.desktop_hide,
			.desktop_hide table {
				display: table !important;
				max-height: none !important;
			}
		}
	</style><!--[if mso ]><style>sup, sub { font-size: 100% !important; } sup { mso-text-raise:10% } sub { mso-text-raise:-10% }</style> <![endif]-->
</head>

<body class="body" style="background-color: #FFFFFF; margin: 0; padding: 0; -webkit-text-size-adjust: none; text-size-adjust: none;">
	<table class="nl-container" width="100%" border="0.5" cellpadding="0" cellspacing="0" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #FFFFFF;">
		<tbody>
			<tr>
				<td>
					<table class="row row-1" align="center" width="100%" border="0" cellpadding="0" cellspacing="0" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #ffffff;">
						<tbody>
							<tr>
								<td>
									<table class="row-content stack" align="center" border="0" cellpadding="0" cellspacing="0" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; width: 525px; margin: 0 auto;" width="525">
										<tbody>
											<tr>
												<td class="column column-1" width="100%" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; padding-bottom: 5px; padding-top: 5px; vertical-align: top;">
													<table class="heading_block block-1" width="100%" border="0" cellpadding="10" cellspacing="0" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt;">
														<tr>
															<td class="pad">
																<h1 style="margin: 0; color: #7747FF; direction: ltr; font-family: 'Abril Fatface', 'Times New Roman', serif; font-size: 43px; font-weight: 400; letter-spacing: normal; line-height: 120%; text-align: center; margin-top: 0; margin-bottom: 0; mso-line-height-alt: 51.6px;"><span class="tinyMce-placeholder" style="word-break: break-word;">Welcome to MyBudgetManagement</span></h1>
															</td>
														</tr>
													</table>
												</td>
											</tr>
										</tbody>
									</table>
								</td>
							</tr>
						</tbody>
					</table>
					<table class="row row-2" align="center" width="100%" border="0" cellpadding="0" cellspacing="0" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #ffffff;">
						<tbody>
							<tr>
								<td>
									<table class="row-content stack" align="center" border="0" cellpadding="0" cellspacing="0" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #ffffff; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-radius: 0; border-right: 1px solid #000000; border-top: 1px solid #000000; color: #000000; width: 525px; margin: 0 auto;" width="525">
										<tbody>
											<tr>
												<td class="column column-1" width="100%" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; background-color: #ffffff; padding-bottom: 5px; padding-top: 5px; vertical-align: top;">
													<table class="image_block block-1" width="100%" border="0" cellpadding="0" cellspacing="0" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt;">
														<tr>
															<td class="pad" style="width:100%;padding-right:0px;padding-left:0px;">
																<div class="alignment" align="center" style="line-height:10px">
																	<div class="fullWidth" style="max-width: 523px;"><img src="https://d15k2d11r6t6rl.cloudfront.net/pub/bfra/4qcmm84c/jvb/idn/5jx/1.png" style="display: block; height: auto; border: 0; width: 100%;" width="523" alt title height="auto"></div>
																</div>
															</td>
														</tr>
													</table>
												</td>
											</tr>
										</tbody>
									</table>
								</td>
							</tr>
						</tbody>
					</table>
					<table class="row row-3" align="center" width="100%" border="0" cellpadding="0" cellspacing="0" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt;">
						<tbody>
							<tr>
								<td>
									<table class="row-content stack" align="center" border="0" cellpadding="0" cellspacing="0" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; width: 525px; margin: 0 auto;" width="525">
										<tbody>
											<tr>
												<td class="column column-1" width="100%" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; padding-bottom: 5px; padding-top: 5px; vertical-align: top;">
													<div class="spacer_block block-1" style="height:20px;line-height:20px;font-size:1px;">&#8202;</div>
													<table class="paragraph_block block-2" width="100%" border="0" cellpadding="10" cellspacing="0" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;">
														<tr>
															<td class="pad">
																<div style="color:#101112;direction:ltr;font-family:Arial, 'Helvetica Neue', Helvetica, sans-serif;font-size:16px;font-weight:400;letter-spacing:0px;line-height:120%;text-align:left;mso-line-height-alt:19.2px;">
																	<p style="margin: 0;"><strong>Hi User,</strong></p>
																</div>
															</td>
														</tr>
													</table>
													<table class="paragraph_block block-3" width="100%" border="0" cellpadding="10" cellspacing="0" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;">
														<tr>
															<td class="pad">
																<div style="color:#101112;direction:ltr;font-family:Arial, 'Helvetica Neue', Helvetica, sans-serif;font-size:16px;font-weight:400;letter-spacing:0px;line-height:120%;text-align:left;mso-line-height-alt:19.2px;">
																	<p style="margin: 0; margin-bottom: 16px;"><strong>Congratulations on starting your journey with MyBudgetManagement.</strong></p>
																	<p style="margin: 0; margin-bottom: 16px;">We are excited to have you on board. Your account has been successfully created. You can now start using our service to enjoy the features and benefits we offer.</p>
																	<p style="margin: 0;">If you have any questions, feel free to contact our support team.</p>
																</div>
															</td>
														</tr>
													</table>
													<div class="spacer_block block-4" style="height:20px;line-height:20px;font-size:1px;">&#8202;</div>
													<table class="button_block block-5" width="100%" border="0" cellpadding="10" cellspacing="0" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt;">
														<tr>
															<td class="pad">
																<div class="alignment" align="center"><a href="https://mybudgetmanagement.nguyenvanlinh.io.vn/login" target="_blank" style="color:#ffffff;text-decoration:none;"><!--[if mso]>
<v:roundrect xmlns:v="urn:schemas-microsoft-com:vml" xmlns:w="urn:schemas-microsoft-com:office:word"  href="https://mybudgetmanagement.nguyenvanlinh.io.vn/login"  style="height:54px;width:164px;v-text-anchor:middle;" arcsize="8%" fillcolor="#7747FF">
<v:stroke dashstyle="Solid" weight="0px" color="#7747FF"/>
<w:anchorlock/>
<v:textbox inset="0px,0px,0px,0px">
<center dir="false" style="color:#ffffff;font-family:sans-serif;font-size:22px">
<![endif]--><span class="button" style="background-color: #7747FF; border-bottom: 0px solid transparent; border-left: 0px solid transparent; border-radius: 4px; border-right: 0px solid transparent; border-top: 0px solid transparent; color: #ffffff; display: inline-block; font-family: Arial, 'Helvetica Neue', Helvetica, sans-serif; font-size: 22px; font-weight: 400; mso-border-alt: none; padding-bottom: 5px; padding-top: 5px; padding-left: 20px; padding-right: 20px; text-align: center; width: auto; word-break: keep-all; letter-spacing: normal;"><span style="word-break: break-word; line-height: 44px;">Explore Now</span></span><!--[if mso]></center></v:textbox></v:roundrect><![endif]--></a></div>
															</td>
														</tr>
													</table>
													<table class="social_block block-6" width="100%" border="0" cellpadding="10" cellspacing="0" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt;">
														<tr>
															<td class="pad">
																<div class="alignment" align="center">
																	<table class="social-table" width="72px" border="0" cellpadding="0" cellspacing="0" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; display: inline-block;">
																		<tr>
																			<td style="padding:0 2px 0 2px;"><a href="https://www.facebook.com/nvl.1712" target="_blank"><img src="https://app-rsrc.getbee.io/public/resources/social-networks-icon-sets/t-only-logo-dark-gray/facebook@2x.png" width="32" height="auto" alt="Facebook" title="facebook" style="display: block; height: auto; border: 0;"></a></td>
																			<td style="padding:0 2px 0 2px;"><a href="mailto:mailto:vanlinhnguyen1729@gmail.com?subject=Hi, I need your support!" target="_blank"><img src="https://app-rsrc.getbee.io/public/resources/social-networks-icon-sets/t-only-logo-dark-gray/mail@2x.png" width="32" height="auto" alt="E-Mail" title="E-Mail" style="display: block; height: auto; border: 0;"></a></td>
																		</tr>
																	</table>
																</div>
															</td>
														</tr>
													</table>
													<table class="paragraph_block block-7" width="100%" border="0" cellpadding="10" cellspacing="0" role="presentation" style="mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;">
														<tr>
															<td class="pad">
																<div style="color:#101112;direction:ltr;font-family:Arial, 'Helvetica Neue', Helvetica, sans-serif;font-size:16px;font-weight:400;letter-spacing:0px;line-height:120%;text-align:center;mso-line-height-alt:19.2px;">
																	<p style="margin: 0; margin-bottom: 16px;">© 2025 MyBudgetManagement. All rights reserved.</p>
																	<p style="margin: 0;">Hanoi, Vietnam</p>
																</div>
															</td>
														</tr>
													</table>
												</td>
											</tr>
										</tbody>
									</table>
								</td>
							</tr>
						</tbody>
					</table>
				</td>
			</tr>
		</tbody>
	</table><!-- End -->
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
