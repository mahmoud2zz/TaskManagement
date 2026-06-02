
using TaskManagement.Domain.Entities;
namespace TaskManagement.Infrastructure.RepositoriesUser
{
	public interface IUserRepository
	{
        Task<List<User>> GetAllAsync();

        //Task<User> AddAsync(User user);
    }
}

