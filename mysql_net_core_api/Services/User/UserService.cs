﻿using AutoMapper;
using mysql_net_core_api.Core.Entitites;
using mysql_net_core_api.Core.Helpers;
using mysql_net_core_api.Core.Interfaces;
using mysql_net_core_api.DTOs.Order;
using mysql_net_core_api.DTOs.User;
using mysql_net_core_api.Repositories;
using mysql_net_core_api.Services.User;

namespace mysql_net_core_api.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cache;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, ICacheService cache, ILogger<UserService> logger, IMapper mapper)
        {
           _unitOfWork=unitOfWork;
            _cache = cache;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<UserDto> CreateUserAsync(UserRegisterDto dto)
        {
            try
            {
                var user = _mapper.Map<UserEntity>(dto);
                await _unitOfWork.Repository<UserEntity>().AddAsync(user);
                _logger.LogInformation($"New User created with id {user.Id}");

                string cacheKey = CachceKeyHelper.GetCacheKey("User", user.Id);
                await _cache.SetAsync(cacheKey, user, TimeSpan.FromMinutes(5));
                return _mapper.Map<UserDto>(user);
            }
            catch (Exception e)
            {

                _logger.LogError($"An error occured while creating a new user: {e.Message}");
                throw;
            }
        }

        public async Task DeleteUserById(Guid id)
        {
            
            try
            {

                string cacheKey = CachceKeyHelper.GetCacheKey("User", id);
                await _cache.RemoveAsync(cacheKey);
                _logger.LogInformation("User removed from cache with id: {id}", id);
                await _unitOfWork.Repository<UserEntity>().DeleteAsync(id);
                _logger.LogInformation("User removed from db with id: {id}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while removing user {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            _logger.LogInformation("Fetching all users from database at {Time}", DateTime.UtcNow);
            try
            {
                var users = await _unitOfWork.Repository<UserEntity>().GetAllAsync();
                if (users == null || !users.Any())
                {
                    _logger.LogWarning("No users found in database at {Time}", DateTime.UtcNow);
                    return new List<UserDto>();
                }
                _logger.LogInformation("{Count} users retrieved successfully at {Time}", users.Count(), DateTime.UtcNow);
                var userDtos = _mapper.Map<List<UserDto>>(users);
                _logger.LogInformation("{Count} users mapped to UserDto successfully at {Time}", users.Count(), DateTime.UtcNow);
                return userDtos;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error while fetching users from database at {Time}", DateTime.UtcNow);
                throw;
            }
        }

        public async Task<UserDto> GetByIdAsync(Guid id)
        {
            try
            {
            string cacheKey = CachceKeyHelper.GetCacheKey("User", id);
            var cachedUser = await _cache.GetAsync<UserDto>(cacheKey);
            if (cachedUser != null)
            {
                _logger.LogInformation("User {id} found in cache", id);
                return cachedUser;
            }
                var user = await _unitOfWork.Repository<UserEntity>().GetByPropAsync(user=>user.Id==id);
                if (user == null)
                {
                    throw new Exception($"User {user!.Id} not found");
                }

                var mappedUser = _mapper.Map<UserDto>(user);
                await _cache.SetAsync(cacheKey, mappedUser, TimeSpan.FromMinutes(2));
                _logger.LogInformation("User {id} set to cache", user.Id);
                return mappedUser;


            }
            catch (Exception)
            {
                _logger.LogError("Error while getting user {id}", id);
                throw;
            }
        }

        public async Task<ICollection<OrderDto>> GetOrdersAsync(string email)
        {
            var user = await _unitOfWork.Repository<UserEntity>().GetByPropAsync(u => u.Email == email);
            try
            {
                var orders = await _unitOfWork.Repository<OrderEntity>().GetWhereAsync(o => o.UserId == user.Id);
                _logger.LogInformation("Orders getting by userId: {id}", user.Id);
                return _mapper.Map<ICollection<OrderDto>>(orders);
            }
            catch (Exception)
            {
                _logger.LogError("An error occured while getting orders by userId:{id}", user.Id);
                throw;
            }
        }

    }
}