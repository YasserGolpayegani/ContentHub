using FluentValidation.Results;

namespace ContentHub.Application.Exceptions;

public sealed class ValidationException : Exception
{
	public IDictionary<string, string[]> Errors { get; }

	public ValidationException(IEnumerable<ValidationFailure> failures)
		:base("One or more validation errors occurred.")
	{
		Errors = failures
			.GroupBy(f => f.PropertyName)
			.ToDictionary(
			g => g.Key,
			g => g.Select(f => f.ErrorMessage).ToArray());
	}

	public ValidationException(string message)
		:base(message)
	{
		Errors = new Dictionary<string, string[]>();
	}
}

