using FluentValidation;

namespace Uni.Web.API.Features.Departments.Commands.DeleteDepartment;
// AbstractValidator<Command> inherits the validation machinery for this Command.
public sealed class Validator : AbstractValidator<Command>
{
    public Validator()
    {
        // RuleFor(...).NotEmpty() says the id must not be the empty Guid
        RuleFor(x => x.DepartmentId).NotEmpty();
    }
}