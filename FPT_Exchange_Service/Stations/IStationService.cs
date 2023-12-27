using FPT_Exchange_Data.DTO.Request.Post;
using FPT_Exchange_Data.DTO.Request.Put;
using Microsoft.AspNetCore.Mvc;

namespace FPT_Exchange_Service.Stations
{
    public interface IStationService
    {
        Task<IActionResult> GetStation(Guid id);
        Task<IActionResult> GetStations();
        Task<IActionResult> CreateStation(CreateStationRequest request);
        Task<IActionResult> UpdateStation(Guid id, UpdateStationRequest request);
    }
}
