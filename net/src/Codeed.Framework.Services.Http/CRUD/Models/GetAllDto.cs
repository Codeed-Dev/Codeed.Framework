namespace Codeed.Framework.Services.Http.CRUD.Models
{
    public class GetAllDto<T>
    {
        public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();
    }
}
