using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Helpers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Enums;
using MyBudgetManagement.Persistance.Context;

namespace MyBudgetManagement.Infrastructure;

public class DataSeeder : IDataSeeder
{
    private readonly ApplicationDbContext _dbContext;

    public DataSeeder(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SeedAsync()
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            await SeedPermissionsAsync();
            await SeedRolesAsync();
            await SeedUsersAsync();
            await SeedUserRolesAsync();
            await SeedAccountProfilesAsync();
            await SeedUserBalanceAsync();
            await transaction.CommitAsync();
            Console.WriteLine("Seed dữ liệu hoàn tất!");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine($"Lỗi khi seed dữ liệu: {ex.Message}");
        }
    }

    private async Task SeedPermissionsAsync()
    {
        if (await _dbContext.Permissions.AnyAsync()) return; // Nếu đã có dữ liệu thì không seed nữa

        var permissions = new[]
        {
            new Permission() { Id = "CREATE", BitValue = 1 },  // 2^0 = 1
            new Permission() { Id = "UPDATE", BitValue = 2 },  // 2^1 = 2
            new Permission() { Id = "DELETE", BitValue = 4 },  // 2^2 = 4
            new Permission() { Id = "VIEW", BitValue = 8 },    // 2^3 = 8
            new Permission() { Id = "APPROVE", BitValue = 16 }, // 2^4 = 16
            new Permission() { Id = "IMPORT", BitValue = 32 }, // 2^5 = 32
            new Permission() { Id = "EXPORT", BitValue = 64 }, // 2^6 = 64
        };

        await _dbContext.Permissions.AddRangeAsync(permissions);
        await _dbContext.SaveChangesAsync();
        Console.WriteLine("Seed Permissions thành công.");
    } 
    
    private async Task SeedRolesAsync()
    {
        if (await _dbContext.Roles.AnyAsync()) return;

        var roles = new[]
        {
            new Role { Name = "User", CreatedBy = "Admin", LastModifiedBy = "Admin", Created = DateTime.Now, RoleBitMask = 107}, // khong co quyen xoa,approve
            new Role { Name = "Admin", CreatedBy = "Admin", LastModifiedBy = "Admin", Created = DateTime.Now ,RoleBitMask = 127}, //full permission
            new Role { Name = "GroupLeader", CreatedBy = "Admin", LastModifiedBy = "Admin", Created = DateTime.Now, RoleBitMask = 123}// khong co quyen xoa
        };

        await _dbContext.Roles.AddRangeAsync(roles);
        await _dbContext.SaveChangesAsync();
        Console.WriteLine("Seed Roles thành công.");
    }

    private async Task SeedUsersAsync()
    {
        var roles = await _dbContext.Roles.ToDictionaryAsync(r => r.Name);

        var existingUsers = await _dbContext.Users
            .Where(u => u.Email == "admin@admin.com" || u.Email == "user@user.com")
            .ToListAsync();

        var usersToAdd = new List<User>();

        if (!existingUsers.Any(u => u.Email == "admin@admin.com"))
        {
            usersToAdd.Add(new User
            {
                Email = "admin@admin.com",
                FirstName = "Linh",
                LastName = "Nguyen",
                PasswordHash = BCryptHelper.HashPassword("admin"),
                CreatedBy = "Admin",
                Created = DateTime.Now,
                LastModifiedBy = "Admin",
            });
        }

        if (!existingUsers.Any(u => u.Email == "user@user.com"))
        {
            usersToAdd.Add(new User
            {
                Email = "user@user.com",
                FirstName = "Linh",
                LastName = "Nguyen",
                PasswordHash = BCryptHelper.HashPassword("user"),
                CreatedBy = "Admin",
                Created = DateTime.Now,
                LastModifiedBy = "Admin",
            });
        }

        if (usersToAdd.Count > 0)
        {
            await _dbContext.Users.AddRangeAsync(usersToAdd);
            await _dbContext.SaveChangesAsync();
            Console.WriteLine("Seed Users thành công.");
        }
        else
        {
            Console.WriteLine("Users đã tồn tại, bỏ qua seed.");
        }
    }
    private async Task SeedUserRolesAsync()
    {
        var users = await _dbContext.Users
            .Where(u => u.Email == "admin@admin.com" || u.Email == "user@user.com")
            .ToListAsync();

        var roles = await _dbContext.Roles.ToDictionaryAsync(r => r.Name);

        var userRolesToAdd = new List<UserRole>();

        var existingUserRoles = await _dbContext.UserRoles
            .Where(ur => users.Select(u => u.Id).Contains(ur.UserId))
            .ToListAsync();

        if (!existingUserRoles.Any(ur => ur.UserId == users.First(u => u.Email == "admin@admin.com").Id))
        {
            userRolesToAdd.Add(new UserRole
            {
                UserId = users.First(u => u.Email == "admin@admin.com").Id,
                RoleId = roles["Admin"].Id,
                Created = DateTime.Now,
                CreatedBy = "Admin",
                LastModifiedBy = "Admin"
            });
        }

        if (!existingUserRoles.Any(ur => ur.UserId == users.First(u => u.Email == "user@user.com").Id))
        {
            userRolesToAdd.Add(new UserRole
            {
                UserId = users.First(u => u.Email == "user@user.com").Id,
                RoleId = roles["User"].Id,
                Created = DateTime.Now,
                CreatedBy = "Admin",
                LastModifiedBy = "Admin"
            });
        }

        if (userRolesToAdd.Count > 0)
        {
            await _dbContext.UserRoles.AddRangeAsync(userRolesToAdd);
            await _dbContext.SaveChangesAsync();
            Console.WriteLine("Seed UserRoles thành công.");
        }
        else
        {
            Console.WriteLine("UserRoles đã tồn tại, bỏ qua seed.");
        }
    }


    private async Task SeedAccountProfilesAsync()
    {
        var users = await _dbContext.Users
            .Where(u => u.Email == "admin@admin.com" || u.Email == "user@user.com")
            .ToListAsync();

        if (users.Count < 2)
        {
            Console.WriteLine("Không đủ users để seed AccountProfiles.");
            return;
        }

        var adminUser = users.FirstOrDefault(u => u.Email == "admin@admin.com");
        var userUser = users.FirstOrDefault(u => u.Email == "user@user.com");

        var existingProfiles = await _dbContext.AccountProfiles
            .Where(ap => ap.UserId == adminUser.Id || ap.UserId == userUser.Id)
            .Select(ap => ap.UserId)
            .ToListAsync();

        var profilesToAdd = new List<AccountProfile>();

        if (!existingProfiles.Contains(adminUser.Id))
        {
            profilesToAdd.Add(new AccountProfile
            {
                UserId = adminUser.Id,
                Avatar = null,
                Address = null,
                Created = DateTime.Now,
                CreatedBy = "Admin",
                Currency = Currencies.VND,
                Gender = Gender.Other,
                LastModifiedBy = "Admin"
            });
        }

        if (!existingProfiles.Contains(userUser.Id))
        {
            profilesToAdd.Add(new AccountProfile
            {
                UserId = userUser.Id,
                Avatar = null,
                Address = null,
                Created = DateTime.Now,
                CreatedBy = "Admin",
                Currency = Currencies.VND,
                Gender = Gender.Other,
                LastModifiedBy = "Admin"
            });
        }

        if (profilesToAdd.Count > 0)
        {
            await _dbContext.AccountProfiles.AddRangeAsync(profilesToAdd);
            await _dbContext.SaveChangesAsync();
            Console.WriteLine("Seed AccountProfiles thành công.");
        }
        else
        {
            Console.WriteLine("AccountProfiles đã tồn tại, bỏ qua seed.");
        }
    }
    
    private async Task SeedUserBalanceAsync()
    {
        var users = await _dbContext.Users
            .Where(u => u.Email == "admin@admin.com" || u.Email == "user@user.com")
            .ToListAsync();

        if (users.Count < 2)
        {
            Console.WriteLine("Không đủ users để seed UserBalance.");
            return;
        }

        var adminUser = users.FirstOrDefault(u => u.Email == "admin@admin.com");
        var userUser = users.FirstOrDefault(u => u.Email == "user@user.com");

        var existingUserBalances = await _dbContext.UserBalances
            .Where(ap => ap.UserId == adminUser.Id || ap.UserId == userUser.Id)
            .Select(ap => ap.UserId)
            .ToListAsync();

        var userBalancesToAdd = new List<UserBalance>();

        if (!existingUserBalances.Contains(adminUser.Id))
        {
            userBalancesToAdd.Add(new UserBalance
            {
                UserId = adminUser.Id,
                Balance = 0,
                Created = DateTime.Now,
                CreatedBy = "Admin",
                LastModifiedBy = "Admin"
            });
        }

        if (!existingUserBalances.Contains(userUser.Id))
        {
            userBalancesToAdd.Add(new UserBalance
            {
                UserId = userUser.Id,
                Balance = 0,
                Created = DateTime.Now,
                CreatedBy = "Admin",
                LastModifiedBy = "Admin"
            });
        }

        if (userBalancesToAdd.Count > 0)
        {
            await _dbContext.UserBalances.AddRangeAsync(userBalancesToAdd);
            await _dbContext.SaveChangesAsync();
            Console.WriteLine("Seed UserBalances thành công.");
        }
        else
        {
            Console.WriteLine("UserBalances đã tồn tại, bỏ qua seed.");
        }
    }
}
