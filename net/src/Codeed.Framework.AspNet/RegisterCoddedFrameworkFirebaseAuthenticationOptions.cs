using Microsoft.Extensions.DependencyInjection;

namespace Codeed.Framework.AspNet
{
    public class RegisterCoddedFrameworkFirebaseAuthenticationOptions : ICoddedFrameworkAuthenticationService
    {
        internal RegisterCoddedFrameworkFirebaseAuthenticationOptions(string firebaseProjectId)
        {
            FirebaseProjectId = firebaseProjectId;
        }

        public string FirebaseProjectId { get; }

        public void RegisterServices(IServiceCollection services)
        {
            services.ConfigureFirebaseAuthentication(FirebaseProjectId);
        }
    }
}