using AutoMapper;
using AutoMapper.QueryableExtensions;
using Firebase.Storage;
using FPT_Exchange_Data;
using FPT_Exchange_Data.DTO.Filters;
using FPT_Exchange_Data.DTO.Internal;
using FPT_Exchange_Data.DTO.Request.Post;
using FPT_Exchange_Data.DTO.Request.Put;
using FPT_Exchange_Data.DTO.View;
using FPT_Exchange_Data.Entities;
using FPT_Exchange_Data.Repositories.Stations;
using FPT_Exchange_Data.Repositories.Users;
using FPT_Exchange_Utility.Constants;
using FPT_Exchange_Utility.Extensions;
using FPT_Exchange_Utility.Helpers;
using FPT_Exchange_Utility.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FPT_Exchange_Service.Users
{
    public class UserService : BaseService, IUserService
    {
        private readonly AppSetting _appSettings;

        private readonly IUserRepository _userRepository;
        private readonly IStationRepository _stationRepository;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<AppSetting> appSettings) : base(unitOfWork, mapper)
        {
            _appSettings = appSettings.Value;
            _userRepository = unitOfWork.User;
            _stationRepository = unitOfWork.Station;
        }

        public async Task<AuthModel?> GetAuthUser(Guid id)
        {
            var user = await _userRepository.GetMany(user => user.Id.Equals(id)).Include(user => user.Role).FirstOrDefaultAsync();
            if (user != null)
            {
                return new AuthModel
                {
                    Id = user.Id,
                    Username = user.Name,
                    Role = user.Role.Name
                };
            }
            return null;
        }

        public async Task<AuthViewModel> AuthenticatedUser(AuthRequest auth)
        {
            var user = await _userRepository.GetMany(user => user.Email.Equals(auth.Email) && user.Password.Equals(auth.Password)).Include(user => user.Role).FirstOrDefaultAsync();
            if (user != null)
            {
                var token = JwtHelper.GenerateJwtToken(user.Id.ToString(), user.Name, user.RoleId.ToString(), _appSettings.Secret);

                return new AuthViewModel
                {
                    AccessToken = token
                };
            }

            return null!;
        }


        public async Task<IActionResult> GetUser(Guid id)
        {
            var customer = await _userRepository.GetMany(customer => customer.Id.Equals(id))
                                                .ProjectTo<UserViewModel>(_mapper.ConfigurationProvider)
                                                .FirstOrDefaultAsync();
            if (customer != null)
            {
                return new JsonResult(customer);
            }
            return new StatusCodeResult(StatusCodes.Status404NotFound);
        }

        public async Task<IActionResult> GetUsers(UserFilterModel filter)
        {
            var query = _userRepository.GetAll();
            if (filter.Name != null)
            {
                query = query.Where(user => user.Name.Contains(filter.Name));
            }
            if (filter.Email != null)
            {
                query = query.Where(user => user.Email.Contains(filter.Email));
            }
            var users = await query.ProjectTo<UserViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            return new JsonResult(users);
        }


        public async Task<IActionResult> RegisterStaff(RegisterStaffRequest request)
        {
            if (_userRepository.Any(user => user.Email.Equals(request.Email)))
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }
            if (!_stationRepository.Any(station => station.Id.Equals(request.StationId)))
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }

            var staff = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                Password = request.Password,
                StationId = request.StationId,
                RoleId = Guid.Parse(UserRole.Staff.GetEnumMember()),
                Status = UserStatus.Active.ToString(),
            };


            _userRepository.Add(staff);
            var result = await _unitOfWork.SaveChanges();
            if (result > 0)
            {
                return await GetUser(staff.Id);
            }
            return new StatusCodeResult(StatusCodes.Status400BadRequest);
        }


        public async Task<IActionResult> AddAvatar(Guid id, IFormFile file)
        {
            var user = await _userRepository.GetMany(user => user.Id.Equals(id)).FirstOrDefaultAsync();
            if (user != null)
            {
                user.Avatar = await UploadProductImageToFirebase(file);
                _userRepository.Update(user);
                var result = await _unitOfWork.SaveChanges();
                if (result > 0)
                {
                    return await GetUser(user.Id);
                }
            }
            return new StatusCodeResult(StatusCodes.Status400BadRequest);
        }


        public async Task<IActionResult> UpdateStaff(Guid id, UpdateStaffRequest request)
        {
            var staff = await _userRepository.GetMany(user => user.Id.Equals(id)).FirstOrDefaultAsync();
            if (staff == null)
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }

            staff.Name = request.Name ?? staff.Name;
            staff.Password = request.Password ?? staff.Password;
            staff.Status = request.Status ?? staff.Status;

            if (request.StationId != null)
            {
                var station = await _stationRepository.GetMany(station => station.Id.Equals(request.StationId)).FirstOrDefaultAsync();
                if (station != null)
                {
                    staff.StationId = request.StationId;

                }
                else
                {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }

            _userRepository.Update(staff);

            var result = await _unitOfWork.SaveChanges();
            if (result > 0)
            {
                return await GetUser(staff.Id);
            }
            return new StatusCodeResult(StatusCodes.Status400BadRequest);
        }


        public async Task<IActionResult> DeleteStaff(Guid id)
        {
            var staff = await _userRepository.GetMany(customer => customer.Id.Equals(id)).FirstOrDefaultAsync();
            if (staff == null)
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }

            staff.Status = UserStatus.DeActive.ToString();
            var result = await _unitOfWork.SaveChanges();
            if (result > 0)
            {
                return new StatusCodeResult(StatusCodes.Status204NoContent);
            }
            return new StatusCodeResult(StatusCodes.Status400BadRequest);
        }


        private async Task<string?> UploadProductImageToFirebase(IFormFile file)
        {
            var storage = new FirebaseStorage("prn231-8f6dd.appspot.com");
            var imageName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var imageUrl = await storage.Child("images")
                                        .Child(imageName)
                                        .PutAsync(file.OpenReadStream());
            return imageUrl;
        }
    }
}
