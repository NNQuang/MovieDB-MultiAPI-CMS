using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieService.Entities.Concrete;

namespace MovieService.Data.Mappings
{
    public class MovieMap : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedOnAdd();
            builder.Property(b => b.CreatedDate).IsRequired();
            builder.Property(b => b.IsActive).IsRequired();
            builder.Property(b => b.IsDeleted).IsRequired();
            builder.Property(b => b.Note).HasMaxLength(300);
            builder.Property(b => b.CreatedByName).HasMaxLength(50).IsRequired();
            builder.Property(b => b.ModifiedByName).HasMaxLength(50);

            builder.Property(m => m.ImdbId).IsRequired();
            builder.Property(m => m.MovieTitle).IsRequired().HasMaxLength(300);
            builder.Property(m => m.ReleaseDate).IsRequired();
            builder.Property(m => m.Duration).IsRequired();
            builder.Property(m => m.Plot).IsRequired().HasMaxLength(10000);
            builder.Property(m => m.Language).IsRequired().HasMaxLength(500);
            builder.Property(m => m.PictureUrl).IsRequired();
            builder.Property(m => m.ImdbRating).IsRequired();
            builder.Property(m => m.RottenTomatoesRating).IsRequired();
            builder.Property(m => m.MetaCriticRating).IsRequired();
            builder.HasOne(m => m.Director).WithMany(d => d.Movies).HasForeignKey(m => m.DirectorId).OnDelete(DeleteBehavior.SetNull);
        }
    }
}