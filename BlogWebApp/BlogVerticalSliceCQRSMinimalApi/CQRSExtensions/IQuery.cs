using MediatR;

namespace BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

public interface IQuery<out TResponse> : IRequest<TResponse>
    where TResponse : notnull;
