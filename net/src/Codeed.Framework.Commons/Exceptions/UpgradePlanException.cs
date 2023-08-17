namespace Codeed.Framework.Commons.Exceptions
{
    public class UpgradePlanException : ServiceException
    {
        public UpgradePlanException(string errorMessage) : base(errorMessage, "upgrade-plan")
        {
        }
    }
}
