using MediatR;

namespace BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

/// <summary>
/// 
/// </summary>
//will not return any response
public interface ICommand : ICommand<Unit>;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TResponse"></typeparam>
//will return TResponse
public interface ICommand<out TResponse> : IRequest<TResponse>;
