using MediatR;

namespace BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

//will not return any response
public interface ICommandHandler<in TCommand>
    : IRequestHandler<TCommand, Unit>
    where TCommand : ICommand<Unit>;

//will return TResponse
public interface ICommandHandler<in TCommand, TResponse>
    : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
    where TResponse : notnull;