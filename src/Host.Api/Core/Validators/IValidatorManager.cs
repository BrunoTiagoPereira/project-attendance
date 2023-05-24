namespace Host.Api.Core.Validators
{
    public interface IValidatorManager
    {
        void ThrowIfInvalid<T>(T @object) where T : ICanBeValidated;
    }
}
