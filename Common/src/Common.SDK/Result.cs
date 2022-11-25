namespace Common.SDK;

public class Result
{
    public int Status { get; init; }

    public bool IsSuccess => Status >= 200 & Status < 300;

    public IReadOnlyCollection<Error> Errors { get; init; }

    public Result() { }
    public Result(int status)
    {
        Status = status;
        Errors = Array.Empty<Error>();
    }

    public Result(int status, params Error[] errors)
    {
        Status = status;
        Errors = errors;
    }

    public Result(int status, IEnumerable<Error> errors) : this(status, errors.ToArray())
    {
    }

    public static Result Ok()
    {
        return new Result(200);
    }
}

public class Result<T> : Result
{
    public T? Data { get; init; }
    public Result(int status, T data) : base(status)
    {
        Data = data;
    }
    public Result() { }
    public Result(int status, params Error[] errors) : base(status, errors)
    {
    }

    public Result(int status, IEnumerable<Error> errors) : base(status, errors)
    {
    }

    public static Result<T> Ok(T data)
    {
        return new Result<T>(200, data);
    }
    public static Result<T> NotFound()
    {
        return new Result<T>(404);
    }
}

public class Error
{
    public string Field { get; init; }
    public string Code { get; init; }

    public Error(string field, string code)
    {
        Field = field;
        Code = code;
    }
}
