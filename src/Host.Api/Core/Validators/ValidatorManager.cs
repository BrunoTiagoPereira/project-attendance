using FluentValidation;
using Host.Api.Core.Exceptions;

namespace Host.Api.Core.Validators
{
    public class ValidatorManager : IValidatorManager
    {
        private readonly IServiceProvider _serviceProvider;

        public ValidatorManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public void ThrowIfInvalid<T>(T @object) where T : ICanBeValidated
        {
            var validator = _serviceProvider.GetRequiredService<IValidator<T>>();

            var validationResult = validator.Validate(@object);

            if (!validationResult.IsValid)
            {
                throw new DomainException(validationResult.Errors);
            }
        }
    }
}
