using Microsoft.EntityFrameworkCore;
using Reflektiv.Speechless.Core.Domain.Concretes.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reflektiv.Speechless.Infrastucture.Repositories.EntityFramework
{
    public class RepositoryContext : DbContext
    {
        public DbSet<BusinessCard> BusinessCards { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<BusinessCard>()
                .HasKey(card => card.Id);
        }
    }
}
