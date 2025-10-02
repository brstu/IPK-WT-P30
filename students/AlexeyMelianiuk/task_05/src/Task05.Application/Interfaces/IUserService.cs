using Task05.Domain.Entities;

namespace Task05.Application.Interfaces;

public interface IUserService
{
    Task InitializeAdminUserAsync();
}