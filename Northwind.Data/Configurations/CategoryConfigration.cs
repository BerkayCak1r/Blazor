using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Data.Configurations
{

        public class CategoryConfiguration : IEntityTypeConfiguration<Category>
        {
            public void Configure(EntityTypeBuilder<Category> builder)
            {

                // Primary Key
                builder.HasKey(c => c.CategoryID);

                // CategoryID Identity (SQL tarafında auto increment)
                builder.Property(c => c.CategoryID)
                       .ValueGeneratedOnAdd();

                // CategoryName zorunlu, maksimum uzunluk 100
                builder.Property(c => c.CategoryName)
                       .IsRequired()
                       .HasMaxLength(100);
            }
        }
}
