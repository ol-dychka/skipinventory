using System;
using Application.Interfaces;
using Domain;

namespace Infrastructure.Persistence.Repositories;

public class JoinRequestRepository(PsqlDbContext context) : IJoinRequestRepository
{
    private readonly PsqlDbContext _context = context;

    public void Add(JoinRequest request)
    {
        _context.JoinRequests.Add(request);
    }
}
