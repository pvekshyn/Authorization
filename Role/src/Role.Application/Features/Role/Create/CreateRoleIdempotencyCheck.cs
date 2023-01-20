using Common.Application.Idempotency;
using Role.Application.Dependencies;

namespace Role.Application.Features.Role.Create;

public class CreateRoleIdempotencyCheck : IIdempotencyCheck<CreateRole>
{
    private readonly IRoleRepository _roleRepository;

    public CreateRoleIdempotencyCheck(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<bool> IsOperationAlreadyAppliedAsync(CreateRole request, CancellationToken cancellationToken)
    {
        return await _roleRepository
            .AnyAsync(request.Role.Id, cancellationToken);
    }
}
