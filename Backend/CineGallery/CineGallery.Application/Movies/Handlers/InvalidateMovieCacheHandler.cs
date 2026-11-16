using CineGallery.Application.Movies.Caching;
using CineGallery.Application.Movies.Events;
using MediatR;

namespace CineGallery.Application.Movies.Handlers
{
    public sealed class InvalidateMovieCacheHandler(IMovieListCache cache) : INotificationHandler<MovieChangedEvent>
    {
        public async Task Handle(MovieChangedEvent notification, CancellationToken cancellationToken)
        {
            await cache.InvalidateAsync(cancellationToken);
        }
    }
}
