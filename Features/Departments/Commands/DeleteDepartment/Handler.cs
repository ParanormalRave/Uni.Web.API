using Microsoft.EntityFrameworkCore;
using Uni.Web.API.Common;
using Uni.Web.API.Common.Messaging;
using Uni.Web.API.Database;

namespace Uni.Web.API.Features.Departments.Commands.DeleteDepartment;
// Handler(ApplicationDbContext context) asks dependency injection for the database. : ICommandHandler<Command> is the plug the scanner looks for.
public sealed class Handler(ApplicationDbContext context) : ICommandHandler<Command>
{
    public async Task<ServiceResponse> Handle(Command command, CancellationToken cancellationToken)
    {
        try
        {
            var department = await context.Departments
                .FirstOrDefaultAsync(d => d.Id == command.DepartmentId, cancellationToken);
            // FirstOrDefaultAsync finds the department or gives null, which becomes “not found”. I used it instead of DeleteStudent’s FindAsync(id, cancellationToken) because that call can treat the token as a second key value
            if (department is null)
            {
                return new ServiceResponse { Success = false, Message = "Department not found." };
            }

            bool hasStudents = await context.Students
                .AnyAsync(s => s.DepartmentId == command.DepartmentId, cancellationToken);
            // AnyAsync is the safety check. The database has ON DELETE CASCADE from students to departments, so deleting a department would silently delete all its students (and their registrations). This check refuses instead.
            if (hasStudents)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Department still has students. Move or delete them first."
                };
            }

            context.Departments.Remove(department);
            // Remove plus SaveChangesAsync does the delete.
            await context.SaveChangesAsync(cancellationToken);
            return new ServiceResponse { Message = "Department deleted successfully." };
        }
        catch (Exception ex)
        {
            string message = $"{ex.Message} - [{ex.GetBaseException().Message}]";
            Console.WriteLine(message);
            return new ServiceResponse { Success = false, Message = message };
        }
        // catch turns any crash into a readable failure message, like other handlers
    }
}