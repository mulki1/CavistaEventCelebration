using CavistaEventCelebration.Api.Dto.DashBoard;

namespace CavistaEventCelebration.Api.Services.Interface
{
    public interface IDashBoardService
    {
        Task<DashBoardDto> Get();
    }
}
