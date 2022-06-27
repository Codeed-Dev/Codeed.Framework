using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sample.Domain;

namespace Sample.Data.Tables
{
    internal class CustomerTable : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("CUSTOMERS");
            builder.HasIndex(c => c.Code).HasDatabaseName("CUSTOMER_CODE_I");
        }
    }
}
