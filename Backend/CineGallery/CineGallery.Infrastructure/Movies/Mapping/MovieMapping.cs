using CineGallery.Domain.Movies.Constants;
using CineGallery.Domain.Movies.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CineGallery.Infrastructure.Movies.Mapping
{
    public class MovieMapping : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(MovieConstants.NAME_MAX_LENGTH)               
                .IsRequired();

            builder.HasIndex(x => x.Name);
        }
    }
}
