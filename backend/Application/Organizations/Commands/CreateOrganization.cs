using MediatR;
using Domain;
using Application.Interfaces;

namespace Application.Organizations.Commands;

public class CreateOrganization
{
    public record Command(string Name) : IRequest<string>;

    public class Handler(IOrganizationRepository repository) : IRequestHandler<Command, string>
    {
        public async Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            var organization = new Organization(request.Name);

            await repository.Add(organization);
            await repository.SaveChangesAsync(cancellationToken);

            return organization.Id;
        }
    }
}