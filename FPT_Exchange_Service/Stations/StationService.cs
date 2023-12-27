using AutoMapper;
using AutoMapper.QueryableExtensions;
using FPT_Exchange_Data;
using FPT_Exchange_Data.DTO.Request.Post;
using FPT_Exchange_Data.DTO.Request.Put;
using FPT_Exchange_Data.DTO.View;
using FPT_Exchange_Data.Entities;
using FPT_Exchange_Data.Repositories.Stations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FPT_Exchange_Service.Stations
{
    public class StationService : BaseService, IStationService
    {
        private readonly IStationRepository _stationRepository;
        public StationService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _stationRepository = _unitOfWork.Station;
        }

        public async Task<IActionResult> GetStation(Guid id)
        {
            var station = await _stationRepository.GetMany(station => station.Id.Equals(id))
                                                    .ProjectTo<StationViewModel>(_mapper.ConfigurationProvider)
                                                    .FirstOrDefaultAsync();
            if(station != null)
            {
                return new JsonResult(station);
            }
            return new StatusCodeResult(StatusCodes.Status404NotFound);
        }

        public async Task<IActionResult> GetStations()
        {
            var station = await _stationRepository.GetAll().ProjectTo<StationViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            return new JsonResult(station);
        }

        public async Task<IActionResult> CreateStation(CreateStationRequest request)
        {
            var station = new Station
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Address = request.Address,
            };
            _stationRepository.Add(station);

            var result = await _unitOfWork.SaveChanges();
            if(result > 0)
            {
                return await GetStation(station.Id);
            }
            return new StatusCodeResult(StatusCodes.Status400BadRequest);
        }

        public async Task<IActionResult> UpdateStation(Guid id, UpdateStationRequest request)
        {
            var station = await _stationRepository.GetMany(station => station.Id.Equals(id)).FirstOrDefaultAsync();
            if(station == null)
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }

            station.Name = request.Name ?? station.Name;
            station.Address = request.Address ?? station.Address;

            _stationRepository.Update(station);

            var result = await _unitOfWork.SaveChanges();
            if (result > 0)
            {
                return await GetStation(station.Id);
            }
            return new StatusCodeResult(StatusCodes.Status400BadRequest);
        }
    }
}
