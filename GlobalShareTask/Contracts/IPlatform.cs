using GlobalShareTask.Models;

namespace GlobalShareTask.Contracts
{
    public interface IPlatform
    {
        LandingStatus GetLandingStatus(Point position);
    }
}