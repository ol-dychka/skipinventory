using System;
using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class JoinRequestRepository(PsqlDbContext context) : IJoinRequestRepository
{
    private readonly PsqlDbContext _context = context;

    public void Add(JoinRequest request)
    {
        _context.JoinRequests.Add(request);
    }

    public Task<JoinRequest?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        return _context.JoinRequests
        // .Include(jr => jr.User)
        // .Include(jr => jr.Organization)
        .FirstOrDefaultAsync(jr => jr.Id == id, cancellationToken: cancellationToken);
    }
}
