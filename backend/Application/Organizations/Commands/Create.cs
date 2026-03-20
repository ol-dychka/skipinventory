using MediatR;
using Domain;
using Application.Interfaces;

namespace Application.Organizations.Commands;

public class Create
{
    public record Command(string Name, string CreatedBy) : IRequest<string>;

    public class Handler(IOrganizationRepository repository) : IRequestHandler<Command, string>
    {
        public async Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            var organization = new Organization(request.Name, request.CreatedBy);

            repository.Add(organization);
            await repository.SaveChangesAsync(cancellationToken);

            return organization.Id;
        }
    }
}