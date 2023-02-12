namespace Codeed.Framework.Domain.Validations
{
    public interface ICreateOrUpdateValidation<T> : ICreateValidation<T>, IUpdateValidation<T>
    {
    }
}
