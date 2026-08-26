namespace BookingApi.API.Extensions
{
    public static class ResultStatusExtensions
    {
        public static int ToHttpStatusCode(this Ardalis.Result.ResultStatus status)
        {
            return status switch
            {
                Ardalis.Result.ResultStatus.Ok => 200,
                Ardalis.Result.ResultStatus.NotFound => 404,
                Ardalis.Result.ResultStatus.Invalid => 400,
                Ardalis.Result.ResultStatus.Unauthorized => 401,
                Ardalis.Result.ResultStatus.Forbidden => 403,
                Ardalis.Result.ResultStatus.Conflict => 409,
                _ => 500
            };
        }
        
    }
}