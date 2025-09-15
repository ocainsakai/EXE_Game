using System.Threading.Tasks;

/// <summary>
/// This interface is used to define an asynchronous use case that returns a response.
/// </summary>
/// <typeparam name="TRequest">The type of the request parameter.</typeparam>
/// <typeparam name="TResponse">The type of the response parameter.</typeparam>
public interface IAsyncUseCase<in TRequest, TResponse>
{
    /// <summary>
    /// Executes the use case asynchronously and returns a response.
    /// </summary>
    /// <param name="request">The request parameter.</param>
    /// <returns>A task representing the asynchronous operation, with a response of type <typeparamref name="TResponse"/>.</returns>
    Task<TResponse> ExecuteAsync(TRequest request);
}