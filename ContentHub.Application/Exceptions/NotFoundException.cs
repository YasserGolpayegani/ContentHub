namespace ContentHub.Application.Exceptions;

/// <summary>
/// Represents a resource that was not found.
/// </summary>
public sealed class NotFoundException : Exception
{
	public NotFoundException(string message):base(message)
	{

	}
}

