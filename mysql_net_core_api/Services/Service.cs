using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using mysql_net_core_api.Core.Entitites;
using mysql_net_core_api.Core.Interfaces;
using mysql_net_core_api.Repositories;

namespace mysql_net_core_api.Services
{
    public class Service<T> : IService<T> where T : class, IEntity<Guid>
    {
        private readonly IRepository<T> _repo;
        private readonly ICacheService _cache;
        private readonly ILogger<Service<T>> _logger;
        private readonly IMapper _mapper;

        public Service(IRepository<T> repo,ICacheService cache,ILogger<Service<T>> logger,IMapper mapper)
        { 
            _repo = repo; 
            _cache = cache; 
            _logger = logger; 
            _mapper = mapper; 
        }

        public async Task AddAsync(T entity)
        {
            var entityType = entity.GetType().Name;
            _logger.LogInformation("AddAsync started for entity type: {entityType}, id : {id}", entityType, entity.Id);
            try
            {
                await _repo.AddAsync(entity);
                _logger.LogInformation("Entity added to db.");

                string cacheKey = $"{entityType}_{entity.Id}";
                await _cache.SetAsync(cacheKey, entity, TimeSpan.FromMinutes(5));
                _logger.LogInformation("Entity added to cache.");
            }
            catch (Exception e )
            {
                _logger.LogError($"An error occured while creating a new {entityType}:{e}");
                throw;
            }
            finally
            {
                _logger.LogInformation("AddAsync ended for entity type: {entityType}, id : {id}", entityType, entity.Id);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            
            try
            {
                var entity = await _repo.GetByIdAsync(id);
                if (entity == null)
                {
                    throw new KeyNotFoundException("Not found");
                }
                string entityType = entity.GetType().Name;
                await _repo.DeleteAsync(entity);
                _logger.LogInformation("{entityType}, id : {id} deleted from db.",entityType , entity.Id);

                string cacheKey = $"{entityType}_{id}";
                await _cache.RemoveAsync(cacheKey);
                _logger.LogInformation("{entityType}, id : {id} deleted from cache.", entityType, entity.Id);

            }
            catch (Exception)
            {
                _logger.LogError("An error occured while deleting id : {id}", id);
                throw;
            }

        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Getting all {entity}s from db. ", typeof(T).Name);
                return await _repo.GetAllAsync();
            }
            catch (Exception ex )
            {
                _logger.LogError("An error occured while getting all {entity}s from db.{ex}", typeof(T).Name,ex);
                throw;
            }
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            string cacheKey = $"{typeof(T).Name}_{id}";
            try
            {
                var entity=  await _cache.GetAsync<T>(cacheKey);
                if (entity!=null)
                {
                    _logger.LogInformation("Entity found in cache with cacheKey:{cacheKey}", cacheKey);
                    return entity;
                }
                _logger.LogInformation("Entity getting from db with id: {id}.",id);
                var newEntity=  await _repo.GetByIdAsync(id);
                _logger.LogInformation("Entity adding to cache with cacheKey: {cacheKey}.",cacheKey);
                await _cache.SetAsync(cacheKey, newEntity, TimeSpan.FromMinutes(5));
                return newEntity;
            }
            catch (Exception)
            {
                _logger.LogError("An error occured while getting {entity} with id: {id}",typeof(T).Name,id);
                throw;
            }
        }

        public async Task UpdateAsync(T entity)
        {
            string cacheKey = $"{entity.GetType().Name}_{entity.Id}";
            try
            {
                
                await _repo.UpdateAsync(entity);
                _logger.LogInformation("Entity updated on database with id: {id}.", entity.Id);
                await _cache.SetAsync(cacheKey, entity, TimeSpan.FromMinutes(5));
                _logger.LogInformation("Entity updated on cache with id: {id}.", entity.Id);

            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while updating {entity} with id:{id}. Exception: {ex}",entity,entity.Id,ex);
                throw;
            }
        }
    }
}
