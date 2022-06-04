namespace Influencer.Core.Exceptions
{
    public interface IHttpException
    {
        int HttpCode { get; }

    }
}
