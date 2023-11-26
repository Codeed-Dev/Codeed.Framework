namespace Codeed.Framework.Specification
{
    public interface ISpecification<T> 
    {
        bool IsSatisfiedBy(T entity);
    }
}