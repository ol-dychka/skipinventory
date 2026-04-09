using System;
using Domain;

namespace Application.Interfaces;

public interface IJoinRequestRepository
{
    void Add(JoinRequest joinRequest);
    Task<JoinRequest?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<JoinRequest?> GetByCredentialsAsync(
        string userId,
        string organizationId,
        CancellationToken cancellationToken
    );
}
