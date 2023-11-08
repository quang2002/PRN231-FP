namespace FP_FAP.Repositories;

using FP_FAP.Models;
using FP_FAP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    public UserRepository(ProjectDbContext projectDbContext)
    {
        this.ProjectDbContext = projectDbContext;
    }

    private ProjectDbContext ProjectDbContext { get; }

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        email = email.ToLower();

        return this
               .ProjectDbContext
               .Users
               .FirstOrDefaultAsync(
                   e => e.Email.ToLower() == email,
                   cancellationToken
               );
    }

    public async Task<IEnumerable<Feedback>> GetUserFeedbacksAsync(string email, CancellationToken cancellationToken = default)
    {
        email = email.ToLower();

        var result = this.ProjectDbContext.Feedbacks
                         .Include(e => e.Student)
                         .Include(e => e.Group.Subject)
                         .Include(e => e.Group.Teacher)
                         .Where(e => e.Student.Email.ToLower() == email || e.Group.Teacher.Email.ToLower() == email);

        return await result.ToListAsync(cancellationToken);
    }

    public async Task<User> GetOrCreateByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await this.GetByEmailAsync(email, cancellationToken);
        if (user is not null)
        {
            return user;
        }

        user = new User
        {
            Email = email,
            Role  = Roles.Student,
        };

        await this.ProjectDbContext.Users.AddAsync(user, cancellationToken: cancellationToken);
        await this.ProjectDbContext.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await this.ProjectDbContext.Users.ToListAsync(cancellationToken);
    }
}