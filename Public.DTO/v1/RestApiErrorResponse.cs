using System.Net;

namespace Public.DTO.v1;

/// <summary>
/// Represents the standard response from the REST API when an error occurs.
/// </summary>
public class RestApiErrorResponse
{
    /// <summary>
    /// Gets or sets the HTTP status code that indicates the outcome of the HTTP response.
    /// </summary>
    public HttpStatusCode Status { get; set; }

    /// <summary>
    /// Gets or sets the error message associated with the response.
    /// </summary>
    public string Error { get; set; } = default!;
}