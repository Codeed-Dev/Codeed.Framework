using Codeed.Framework.AspNet.RegisterServicesConfigurations;
using Codeed.Framework.Tenant;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Codeed.Framework.AspNet
{
    public class RegisterCodeedFrameworkOptions
    {
        private readonly ICollection<ICodeedServiceConfiguration> _servicesConfigurations = new List<ICodeedServiceConfiguration>();
        public RegisterCodeedFrameworkOptions(string name, string assemblyPattern)
        {
            Name = name;
            AssemblyPattern = assemblyPattern;
        }

        public string Name { get; }

        public string AssemblyPattern { get; }

        internal IEnumerable<ICodeedServiceConfiguration> ServicesConfigurations => _servicesConfigurations;

        internal Action<DbContextOptionsBuilder> DbContextOptionsBuilder { get; private set; }

        public void ConfigureDatabase(Action<DbContextOptionsBuilder> dbContextOptionsBuilder)
        {
            DbContextOptionsBuilder = dbContextOptionsBuilder;
        }

        public void AddServiceConfiguration(ICodeedServiceConfiguration serviceConfiguration)
        {
            _servicesConfigurations.Add(serviceConfiguration);
        }
    }
}
