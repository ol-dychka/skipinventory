using Application.Core;
using Application.Interfaces;
using Domain;
using Domain.StaticClasses;
using MediatR;

namespace Application.Organizations.Commands;

public class Create
{
    public record Command(string Name, string CreatorId) : IRequest<Result<string>>;

    public class Handler(
        IOrganizationRepository organizationRepository,
        IMemberRepository memberRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork
    ) : IRequestHandler<Command, Result<string>>
    {
        public async Task<Result<string>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            var user = await userRepository.GetByIdAsync(request.CreatorId, cancellationToken);
            if (user == null)
                return Result<string>.Failure("User not found");

            var exists = await organizationRepository.ExistsWithNameAsync(
                request.Name,
                cancellationToken
            );
            if (exists)
                return Result<string>.Failure("Organization with this name already exists");

            var organization = new Organization(request.Name, request.CreatorId);
            organizationRepository.Add(organization);

            var membership = new OrganizationMember(
                request.CreatorId,
                organization.Id,
                UserRole.Owner
            );
            memberRepository.Add(membership);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<string>.Success(organization.Id);
        }
    }
}
