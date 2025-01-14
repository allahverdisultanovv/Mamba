using MambaMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MambaMVC.Configuration
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(e => e.Surname)
                .IsRequired()
                .HasMaxLength(50);
           
        }
    }
}
