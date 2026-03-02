using System;
using Application.Interfaces;
using MediatR;

namespace Application.Organizations.Commands;

public class DeleteOrganization
{
    public record Command(string Id) : IRequest;

    public class Handler(IOrganizationRepository repository) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var organization = await repository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new Exception("Cannot find this organization");

            await repository.Delete(organization);

            await repository.SaveChangesAsync(cancellationToken);
        }
    }
}
