using System;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;

namespace Application.Users.Queries;

public class Details
{
    public record Query(string Id) : IRequest<Result<User>>;

    public class Handler(IUserRepository repository) : IRequestHandler<Query, Result<User>>
    {
        public async Task<Result<User>> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await repository.GetByIdWithOrganizationAsync(request.Id, cancellationToken);
            
            if (user == null) return Result<User>.Failure("user does not exist");

            return Result<User>.Success(user);
        }
    }
}
