namespace IngoX.Client.Bff.Core.Models;

public class ResultModel<T>
{
    private readonly T? _value;

    private ResultModel(T value)
    {
        Value = value;
        IsSuccess = true;
        Error = ErrorModel.None;
    }

    private ResultModel(ErrorModel error)
    {
        if (error == ErrorModel.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = false;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public T Value
    {
        get
        {
            return _value!;
        }

        private init => _value = value;
    }

    public ErrorModel Error { get; }

    public static ResultModel<T> Success(T value)
    {
        return new ResultModel<T>(value);
    }

    public static ResultModel<T> Failure(ErrorModel error)
    {
        return new ResultModel<T>(error);
    }
}
