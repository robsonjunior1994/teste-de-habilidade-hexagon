namespace Hexagon.Api.Presentation.Common
{
    public enum ErrorCode
    {
        // Erros gerais de validação (400)
        VALIDATION_FAILED = 1000,
        REQUIRED_FIELD,

        // Erros específicos de domínio (409, 422, 404)
        RESOURCE_ALREADY_EXISTS,
        INVALID_EMAIL,
        RESOURCE_NOT_FOUND,
        INACTIVE_RESOURCE,
        INVALID_CREDENTIALS,

        // Erros de infraestrutura/inesperados (500)
        DATABASE_ERROR,
        EXTERNAL_SERVICE_UNAVAILABLE

        // ... adicione outros conforme a necessidade
    }
}
