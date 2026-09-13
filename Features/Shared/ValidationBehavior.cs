using FluentValidation;
using MediatR;
using exam_system.Common.Enums;

namespace exam_system.Features.Shared
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }
        public async Task<TResponse> Handle(TRequest request,
                                RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }
            var failures = _validators
                    .Select(Validator => Validator.Validate(request))
                    .SelectMany(result => result.Errors)
                    .Where(failure => failure != null)
                    .GroupBy(f => f.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(f => f.ErrorMessage)
                    .ToArray());

            if (failures.Any())
            {
                var responseType = typeof(TResponse);
                var failMethod = responseType.GetMethod("Fail", new[] { typeof(ErrorType), typeof(string), typeof(IDictionary<string, string[]>) });
                var result = failMethod!.Invoke(null, new object?[] { ErrorType.Validation,"Validation failed" , failures });
                return (TResponse)result!;
            }
            return await next();
        }
      
    }
    
}

