using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vyayaam.Data;
using Vyayaam.Data.Entities;
using Vyayaam.ViewModels;

namespace Vyayaam.Controllers
{
    [Route("api/[Controller]")]
    public class OrdersController : Controller
    {
        private readonly IVyayaamRepository repository;
        private readonly ILogger<OrdersController> logger;
        private readonly IMapper mapper;

        public OrdersController(IVyayaamRepository repository, ILogger<OrdersController> logger, IMapper mapper)
        {
            this.repository = repository;
            this.logger = logger;
            this.mapper = mapper;
        }

        public IActionResult Get()
        {
            try
            {
                return Ok(mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(repository.GetAllOrders()));
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to get all orders, {ex}");
                return BadRequest("Failed to get all orders");
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                var order = repository.GetOrderById(id);
                if (order != null)
                    return Ok(mapper.Map<Order, OrderViewModel>(order));
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to get order, {ex}");
                return BadRequest("Failed to get order");
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]OrderViewModel order)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var newOrder = mapper.Map<OrderViewModel, Order>(order);

                    if(newOrder.OrderDate == DateTime.MinValue)
                    {
                        newOrder.OrderDate = DateTime.Now;
                    }

                    repository.AddEntity(newOrder);
                    if (repository.SaveAll())
                    {
                        var vm = mapper.Map<Order, OrderViewModel>(newOrder);
                        return Created($"/api/order/{vm.OrderId}", vm);
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
                
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to save new order, {ex}");
                return BadRequest($"Failed to save new order");
            }
            return BadRequest("Failed to save new order");
        }
    }
}
