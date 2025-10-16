using MediatR;

namespace BuildingBlocks.CQRS;

// empty interface to represent a command with no response
public interface ICommand : ICommand<Unit> 
{ }

public interface ICommand<out TResponse> : IRequest<TResponse>
{ }
