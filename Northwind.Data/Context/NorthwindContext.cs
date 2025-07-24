using Microsoft.EntityFrameworkCore;
//using System.Collections.Generic;
//using WebApplication1.Models;
using Northwind.Core.Entities; // ← Product ve Category için





namespace Northwind.Data

{
    public class NorthwindContext : DbContext
    {
        public NorthwindContext(DbContextOptions<NorthwindContext> options) : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<Employee> Employees => Set<Employee>();


    }
}
