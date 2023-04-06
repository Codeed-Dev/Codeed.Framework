namespace Codeed.Framework.Commons.Exceptions
{
    public interface IHttpException
    {
        string Code { get; }

        object? Parameters { get; }

        int HttpCode { get; }
    }
}
