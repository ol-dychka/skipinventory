using System;
using Domain;
using Infrastructure.Persistence;
using MediatR;

namespace Application.Organizations.Commands;

public class CreateOrganization
{
    public class Command : IRequest<string>
    {
        public required Organization Organization { get; set; }
    }

    public class Handler(PsqlDbContext context) : IRequestHandler<Command, string>
    {
        public async Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            context.Organizations.Add(request.Organization);

            await context.SaveChangesAsync(cancellationToken);

            return request.Organization.Id;
        }
    }
}
