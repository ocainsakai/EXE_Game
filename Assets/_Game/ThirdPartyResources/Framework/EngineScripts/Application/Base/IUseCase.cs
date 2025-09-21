/// <summary>
/// This interface is used to define a use case that returns a response.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public interface IUseCase<in TRequest, out TResponse>
{
    /// <summary>
    /// Executes the use case with the given request and returns a response.
    /// </summary>
    /// <param name="request">The request to execute the use case with.</param>
    /// <returns>The response from the use case.</returns>
    TResponse Execute(TRequest request);
}