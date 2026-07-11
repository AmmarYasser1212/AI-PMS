using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.Behaviors
{
    public class ValidationBehaviors<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
      where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationBehaviors(IEnumerable<IValidator<TRequest>> validators) =>
            _validators = validators;

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,RequestHandlerDelegate<TResponse> next )
        {
            if(!_validators.Any()) 
                return await next();

            var context = new ValidationContext<TRequest>(request);

            var results = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = results.SelectMany(r=>r.Errors).Where(e=>e is not null).ToList();

            if (failures.Any())
                throw new ValidationException(failures);

            return await next();
        }


    }
}
