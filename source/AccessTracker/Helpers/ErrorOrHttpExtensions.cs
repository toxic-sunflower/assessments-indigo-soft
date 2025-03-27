using ErrorOr;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AccessTracker.Helpers;

public static class ErrorOrHttpExtensions
{
    public static Results<Ok, ProblemHttpResult> ToHttpResult(
        this ErrorOr<Success> errorOrSuccess)
    {
        return errorOrSuccess.Match<Results<Ok, ProblemHttpResult>>(
            _ => TypedResults.Ok(),
            errors => errors.ToHttpProblemResult());
    }
    
    public static Results<Ok<T>, ProblemHttpResult> ToHttpResult<T>(
        this ErrorOr<T> errorOrSuccess)
    {
        return errorOrSuccess.Match<Results<Ok<T>, ProblemHttpResult>>(
            value => TypedResults.Ok(value),
            errors => errors.ToHttpProblemResult());        
    }
    
    public static ProblemHttpResult ToHttpProblemResult(this List<Error> errors)
    {
        return errors.Count switch
        {
            0 => TypedResults.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                detail: "Unknown error",
                type: "Unknown"),

            1 => TypedResults.Problem(
                statusCode: MapStatusCode(errors[0].Type, errors[0].Code),
                detail: errors[0].Description,
                type: errors[0].Code),

            _ => TypedResults.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                detail: "Multiple errors occurred",
                extensions: errors.Select(
                    x => new KeyValuePair<string, object?>(
                        x.Type.ToString(),
                        new { x.Code, x.Description })))
        }; 
    }

    private static int MapStatusCode(ErrorType errorType, string errorCode)
    {
        return errorType switch
        {
            ErrorType.Failure => StatusCodes.Status422UnprocessableEntity,
            ErrorType.Unexpected => StatusCodes.Status500InternalServerError,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}