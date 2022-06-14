using Microsoft.EntityFrameworkCore;
using System;

namespace Codeed.Framework.AspNet
{
    public class RegisterCoddedFrameworkOptions
    {
        public RegisterCoddedFrameworkOptions(string name, string assemblyPattern)
        {
            Name = name;
            AssemblyPattern = assemblyPattern;
        }

        public string Name { get; }

        public string AssemblyPattern { get; }

        internal RegisterCoddedFrameworkSwaggerOptions SwaggerOptions { get; private set; }

        internal ICoddedFrameworkAuthenticationService AuthenticationService { get; private set; }

        internal Action<DbContextOptionsBuilder> DbContextOptionsBuilder { get; private set; }

        public void ConfigureFirebaseAuthentication(string firebaseProjectId)
        {
            ConfigureFirebaseAuthentication(firebaseProjectId, null);
        }

        public void ConfigureFirebaseAuthentication(string firebaseProjectId, Action<RegisterCoddedFrameworkFirebaseAuthenticationOptions> configure)
        {
            var options = new RegisterCoddedFrameworkFirebaseAuthenticationOptions(firebaseProjectId);
            if (configure != null)
            {
                configure(options);
            }

            AuthenticationService = options;
        }

        public void ConfigureSwagger(Action<RegisterCoddedFrameworkSwaggerOptions> configure)
        {
            var options = new RegisterCoddedFrameworkSwaggerOptions();
            if (configure != null)
            {
                configure(options);
            }

            SwaggerOptions = options;
        }

        public void ConfigureDatabase(Action<DbContextOptionsBuilder> dbContextOptionsBuilder)
        {
            DbContextOptionsBuilder = dbContextOptionsBuilder;
        }
    }
}
