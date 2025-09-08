namespace DivineKids.Application.Common;

public sealed record Result
{
    public bool Succeeded { get; }
    public string? Error { get; }

    private Result(bool succeeded, string? error)
    {
        Succeeded = succeeded;
        Error = error;
    }

    public static Result Success() => new(true, null);
    public static Result Failure(string error) => new(false, error);
}

public sealed record Result<T>
{
    public bool Succeeded { get; }
    public string? Error { get; }
    public T? Value { get; }

    private Result(bool succeeded, T? value, string? error)
    {
        Succeeded = succeeded;
        Value = value;
        Error = error;
    }

    public static Result<T> Success(T value) => new(true, value, null);
    public static Result<T> Failure(string error) => new(false, default, error);
}