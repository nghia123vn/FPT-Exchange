using FPT_Exchange_Data.DTO.Request.Post;
using FPT_Exchange_Data.DTO.Request.Put;
using FPT_Exchange_Service.Stations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FPT_Exchange_API.Controllers
{
    [Route("api/stations")]
    [ApiController]
    public class StationsController : ControllerBase
    {
        private readonly IStationService _stationService;

        public StationsController(IStationService stationService)
        {
            _stationService = stationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStations()
        {
            try
            {
                var result = await _stationService.GetStations();
                if(result is JsonResult jsonResult)
                {
                    return Ok(jsonResult.Value);
                }
                return BadRequest("Somethings wrong!!!");
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateStation(CreateStationRequest request)
        {
            try
            {
                var result = await _stationService.CreateStation(request);
                if(result is JsonResult jsonResult)
                {
                    return StatusCode(StatusCodes.Status201Created, jsonResult.Value);
                }
                return BadRequest("Somethings wrong when save to database!!!");
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateStation([FromRoute] Guid id,
                                                        [FromBody] UpdateStationRequest request)
        {
            try
            {
                var result = await _stationService.UpdateStation(id, request);
                if(result is JsonResult jsonResult)
                {
                    return StatusCode(StatusCodes.Status201Created, jsonResult.Value);
                }
                return BadRequest("Somethings wrong when save to database!!!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
