﻿using Application.Abstraction;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.DataAccess;

public class BookCatalogDbContext : DbContext, IBookCatalogDbContext
{
    public BookCatalogDbContext(DbContextOptions<BookCatalogDbContext> options)
        : base(options)
    {

    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshToken { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(option => option.Email).IsUnique();
    }
    public override EntityEntry Update(object entity)
    {
        return base.Update(entity);
    }
}
