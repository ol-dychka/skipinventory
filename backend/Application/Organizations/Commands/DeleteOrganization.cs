using System;
using Domain;
using Infrastructure.Persistence;
using MediatR;

namespace Application.Organizations.Commands;

public class DeleteOrganization
{
    public class Command : IRequest
    {
        public required string Id { get; set; }
    }

    public class Handler(PsqlDbContext context) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var organization = await context.Organizations
                .FindAsync([request.Id], cancellationToken)
                    ?? throw new Exception("Cannot find this organization");

            context.Organizations.Remove(organization);

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
