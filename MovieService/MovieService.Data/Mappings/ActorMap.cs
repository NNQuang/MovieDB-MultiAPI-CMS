using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieService.Entities.Concrete;
using System;
using System.Collections.Generic;

namespace MovieService.Data.Mappings
{
    public class ActorMap : IEntityTypeConfiguration<Actor>
    {
        public void Configure(EntityTypeBuilder<Actor> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedOnAdd();
            builder.Property(b => b.CreatedDate).IsRequired();
            builder.Property(b => b.IsActive).IsRequired();
            builder.Property(b => b.IsDeleted).IsRequired();
            builder.Property(b => b.Note).HasMaxLength(300);
            builder.Property(b => b.CreatedByName).HasMaxLength(50).IsRequired();
            builder.Property(b => b.ModifiedByName).HasMaxLength(50);

            builder.Property(a => a.FullName).IsRequired().HasMaxLength(100);
            builder.Property(a => a.PictureUrl);

            builder.HasData(new Actor
            {
                Id = 1,
                CreatedDate = DateTime.Now,
                FullName = "Test Data",
                IsActive = true,
                IsDeleted = false,
                CreatedByName = "initial create",
                ModifiedByName = "initial create",
                ModifiedDate = DateTime.Now,
                Movies = new List<Movie>(),
                Note = "initial create",
                PictureUrl = "dummy url"
            });
        }
    }
}