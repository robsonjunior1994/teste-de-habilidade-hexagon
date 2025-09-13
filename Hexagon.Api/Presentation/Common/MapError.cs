namespace Hexagon.Api.Presentation.Common
{
    public static class MapError
    {
        public static int MapErrorToStatusCode(ErrorCode errorCode)
        {
            return errorCode switch
            {
                ErrorCode.RESOURCE_ALREADY_EXISTS => StatusCodes.Status409Conflict,
                ErrorCode.INVALID_EMAIL => StatusCodes.Status422UnprocessableEntity,
                ErrorCode.RESOURCE_NOT_FOUND => StatusCodes.Status404NotFound,
                ErrorCode.INVALID_CREDENTIALS => StatusCodes.Status401Unauthorized,
                ErrorCode.DATABASE_ERROR or ErrorCode.EXTERNAL_SERVICE_UNAVAILABLE => StatusCodes.Status503ServiceUnavailable,
                _ => StatusCodes.Status400BadRequest // padrao para outros erros
            };
        }
    }
}
