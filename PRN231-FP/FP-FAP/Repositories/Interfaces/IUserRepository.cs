﻿namespace FP_FAP.Repositories.Interfaces;

using FP_FAP.Models;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string            email,
                                CancellationToken cancellationToken = default
    );

    Task<User> GetOrCreateByEmailAsync(string            email,
                                       CancellationToken cancellationToken = default
    );

    Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<Feedback>> GetUserFeedbacksAsync(string            email,
                                                      CancellationToken cancellationToken = default);
}