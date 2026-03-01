using System;
using AutoMapper;
using Domain;
using Infrastructure.Persistence;
using MediatR;

namespace Application.Organizations.Commands;

public class EditOrganization
{
    public class Command : IRequest
    {
        public required Organization Organization { get; set; }
    }

    public class Handler(PsqlDbContext context, IMapper mapper) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var organization = await context.Organizations
                .FindAsync([request.Organization.Id], cancellationToken)
                    ?? throw new Exception("Cannot find this organization");

            mapper.Map(request.Organization, organization);

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
