using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.Authority = $"https://securetoken.google.com/{FirebaseProjectId}";
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidIssuer = $"https://securetoken.google.com/{FirebaseProjectId}",
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidAudience = FirebaseProjectId,
                            ValidateLifetime = true
                        };
                    });
        }
    }
}