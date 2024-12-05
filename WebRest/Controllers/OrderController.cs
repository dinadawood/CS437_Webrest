using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using WebRestEF.EF.Data;
using WebRestEF.EF.Models;
using WebRest.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Runtime.ConstrainedExecution;
using WebRestShared.DTO;
namespace WebRest.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase, iController<Order, OrderDTO>
    {
        private readonly WebRestOracleContext _context;
        private readonly IMapper _mapper;

        public OrderController(WebRestOracleContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
           // _context.LoggedInUserId = "XYZ";
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> Get()
        {
            return await _context.Orders.ToListAsync();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Order>> Get(string id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, OrderDTO _orderDTO)
        {

            if (id != _orderDTO.OrderId)
            {
                return BadRequest();
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                //  Set context
                //_context.SetUserID(_context.LoggedInUserId);

                //  POJO code goes here                
                var _item = _mapper.Map<Order>(_orderDTO);
                _context.Orders.Update(_item);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Exists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                
                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw new Exception(e.Message, e);
            }

            return NoContent();

        }

        [HttpPost]
        public async Task<ActionResult<Order>> Post(OrderDTO _orderDTO)
        {
            Order _item = _mapper.Map<Order>(_orderDTO);
            _item.OrderId = null;      //  Force a new PK to be created
            _context.Orders.Add(_item);
            await _context.SaveChangesAsync();

            CreatedAtActionResult ret = CreatedAtAction("Get", new { id = _item.OrderId }, _item);
            return Ok(ret);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Exists(string id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }


    }
}
