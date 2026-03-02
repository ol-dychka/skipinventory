using System;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Organizations.Commands;

public class EditOrganization
{
    public record Command(Organization Organization) : IRequest;

    public class Handler(IOrganizationRepository repository, IMapper mapper) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var organization = await repository.GetByIdAsync(request.Organization.Id, cancellationToken)
                ?? throw new Exception("Cannot find this organization");

            mapper.Map(request.Organization, organization);

            await repository.SaveChangesAsync(cancellationToken);
        }
    }
}
