using System;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class PsqlDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<SaleRecord> SaleRecords { get; set; }

}
