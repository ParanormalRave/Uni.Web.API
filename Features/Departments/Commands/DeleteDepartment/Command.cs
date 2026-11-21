using Uni.Web.API.Common.Messaging;
// The using brings in ICommand

namespace Uni.Web.API.Features.Departments.Commands.DeleteDepartment;
// The namespace line must match the folder path


public sealed record Command(Guid DepartmentId) : ICommand;
// record Command(Guid DepartmentId) is a tiny immutable data holder carrying the id.
// : ICommand is the plug. It says “I am a command with no return data”.