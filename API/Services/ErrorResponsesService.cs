using System.Net;
using ClientModels.Errors;

namespace API.Services
{
    public static class ErrorResponsesService
    {
        public static ServiceErrorResponse BodyIsMissing(string target)
        {
            return new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Error = new ServiceError
                {
                    Code =  ServiceErrorCodes.BadRequest,
                    Message = "Request body is empty.",
                    Target = target
                }
            };
        }

        public static ServiceErrorResponse DuplicationError(string target, string message)
        {
            return new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Error = new ServiceError
                {
                    Code =  ServiceErrorCodes.BadRequest,
                    Message = message,
                    Target = target
                }
            };
        }

        public static ServiceErrorResponse NotFoundError(string target, string message)
        {
            return new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.NotFound,
                Error = new ServiceError
                {
                    Code =  ServiceErrorCodes.NotFound,
                    Message = message,
                    Target = target
                }
            };
        }
        
        public static ServiceErrorResponse InvalidImageData(string target)
        {
            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.BadRequest,
                    Message = "Image must has PNG or JPG format and be less than 512 KB.",
                    Target = target
                }
            };

            return error;
        }
        
        public static ServiceErrorResponse InvalidBodyData(string target, string message)
        {
            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.BadRequest,
                    Message = message,
                    Target = target
                }
            };

            return error;
        }
        
        public static ServiceErrorResponse InvalidCredentialsError(string target)
        {
            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.InvalidCredentials,
                    Message = "Username or password is incorrect.",
                    Target = target
                }
            };

            return error;
        }
        
        public static ServiceErrorResponse ValidationError(string target, string message)
        {
            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.ValidationError,
                    Message = message,
                    Target = target
                }
            };

            return error;
        }
        
        public static ServiceErrorResponse UnauthorizedError(string target)
        {
            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.Unauthorized,
                    Message = "You must be authorized. Access prohibited.",
                    Target = target
                }
            };

            return error;
        }
    }
}