using AutoMapper;
using mysql_net_core_api.Core.Entitites;
using mysql_net_core_api.Core.Helpers;
using mysql_net_core_api.Core.Interfaces;
using mysql_net_core_api.DTOs.Order;
using mysql_net_core_api.Repositories;

namespace mysql_net_core_api.Services.Order
{
    public class OrderService:IOrderService
    {
        private IUnitOfWork _unitOfWork;
        private readonly ILogger<OrderService> _logger;
        private readonly ICacheService _cache;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, ILogger<OrderService> logger, ICacheService cache, IMapper mapper)
        {
            _cache = cache;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<OrderDto> CreateOrderAsync(OrderCreateDto dto)
        {
            try
            {
                var order = _mapper.Map<OrderEntity>(dto);
                var cacheKey = CachceKeyHelper.GetCacheKey("Order", order.Id);
                //order.OrderItems.Select(orderItem => orderItem.OrderId = order.Id);

                foreach (var item in order.OrderItems)
                {
                    var product = await _unitOfWork.Repository<ProductEntity>().GetByPropAsync(i => i.Id == item.ProductId);
                    item.TotalPrice = product.Price * item.Quantity;
                }
               order.TotalAmount= order.OrderItems.Sum(o => o.TotalPrice);
                await _unitOfWork.Repository<OrderEntity>().AddAsync(order);
                _logger.LogInformation("Order added with id:{id}", order.Id);
                await _cache.SetAsync(cacheKey, order,TimeSpan.FromMinutes(5));
                _logger.LogInformation("Order added to cache withid:{id}", order.Id);
                var createdDto = _mapper.Map<OrderDto>(order);
                await _unitOfWork.CompleteAsync();
                return createdDto;
            }
            catch (Exception ex )
            {
                _logger.LogError("An error occured while creating order.{ex}", ex);
                throw;
            }
        }

        public async Task DeleteOrderByIdAsync(Guid id)
        {
            try
            {
                await _unitOfWork.Repository<OrderEntity>().DeleteAsync(id);
                _logger.LogInformation("Order deleted with id: {id}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while deleting order.{ex}", ex);
                throw;
            }
        }

        public async Task<OrderDto> GetByIdAsync(Guid id)
        {
            try
            {
                string cacheKey = CachceKeyHelper.GetCacheKey("Order", id);
                var cachedOrder = await _cache.GetAsync<OrderEntity>(cacheKey);
                if (cachedOrder!=null)
                {
                    _logger.LogInformation("Order found in cache with key:{key}", cacheKey);
                    return _mapper.Map<OrderDto>(cachedOrder);
                }
                var dbOrder = await _unitOfWork.Repository<OrderEntity>().GetByPropAsync(order => order.Id == id);
                _logger.LogInformation("Order found in db with id:{id}", id);
                return _mapper.Map<OrderDto>(dbOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while getting order.{ex}", ex);
                throw;
            }
        }

        public async Task UpdateOrderAsync(Guid id, OrderCreateDto dto)
        {
            try
            {
                var order = _mapper.Map<OrderEntity>(dto);
                order.Id = id;
                await _unitOfWork.Repository<OrderEntity>().UpdateAsync(order);
                _logger.LogInformation("Order updated in db with id:{id}", id);
                string cacheKey = CachceKeyHelper.GetCacheKey($"Order", id);
                await _cache.SetAsync(cacheKey, order, TimeSpan.FromMinutes(5));
                _logger.LogInformation("Order updated in cache with id:{id}", id);
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while updating order.{ex}", ex);
                throw;
            }
        }

        
    }
}
