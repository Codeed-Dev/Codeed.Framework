namespace Codeed.Framework.Domain.Validations
{
    public interface IValidationOfT<in T>
    {
        int Priority { get; }

        Task ValidateAsync(T @obj, CancellationToken cancellationToken);
    }
}
