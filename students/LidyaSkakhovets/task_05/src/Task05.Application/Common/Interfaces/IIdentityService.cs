namespace Task05.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string> GetUserNameAsync(string userId);
    Task<bool> IsInRoleAsync(string userId, string role);
    Task<bool> AuthorizeAsync(string userId, string policyName);
    Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);
    Task<Result> DeleteUserAsync(string userId);
    Task CreateRoleAsync(string roleName);
    Task AddToRoleAsync(string userId, string roleName);
}

public record Result(bool Succeeded, IEnumerable<string> Errors)
{
    public static Result Success() => new Result(true, Array.Empty<string>());
    public static Result Failure(IEnumerable<string> errors) => new Result(false, errors);
}