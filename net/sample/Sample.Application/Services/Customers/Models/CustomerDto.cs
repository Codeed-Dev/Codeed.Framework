using Codeed.Framework.Models;
using System.ComponentModel.DataAnnotations;

namespace Sample.Application.Services.Customers.Models
{
    /// <summary>
    /// Customer
    /// </summary>
    public class CustomerDto : IDto
    {
        /// <summary>
        /// Customer code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Customer description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Customer identification
        /// </summary>
        public string Identification { get; set; }
    }
}
