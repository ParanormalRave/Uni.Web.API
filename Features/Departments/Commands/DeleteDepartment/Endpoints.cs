using Uni.Web.API.Common.Endpoints;
using Uni.Web.API.Common.Messaging;

namespace Uni.Web.API.Features.Departments.Commands.DeleteDepartment;

public sealed class Endpoint : IEndpoint
// : IEndpoint plus the MapEndpoint method is the plug for the endpoint scanner.
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/departments/{id:guid}", async (
            // MapDelete(".../{id:guid}") answers DELETE requests, and :guid rejects ids that aren’t valid Guids.
            Guid id,
            ICommandHandler<Command> handler,
            // ICommandHandler<Command> handler is injected by DI, so you never write new Handler(...).
            CancellationToken cancellationToken) =>
        {
            var command = new Command(id);
            var result = await handler.Handle(command, cancellationToken);
            return result.Success ? Results.Ok(result) : Results.NotFound(result);
            // The result is 200 on success, otherwise 404 with the message. (Strictly, “has students” is a 409 Conflict.
        })
        .WithTags(Tags.Departments);
        // .WithTags(Tags.Departments) groups it in Scalar. That constant already exists.
    }
}