using MediatR;

namespace CineGallery.Application.Movies.Events
{
    public record MovieChangedEvent(Guid MovieId) : INotification;
}
