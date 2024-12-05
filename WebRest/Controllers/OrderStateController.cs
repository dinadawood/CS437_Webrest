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
    public class OrderStateController : ControllerBase, iController<OrderState, OrderStateDTO>
    {
        private readonly WebRestOracleContext _context;
        private readonly IMapper _mapper;

        public OrderStateController(WebRestOracleContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
           // _context.LoggedInUserId = "XYZ";
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderState>>> Get()
        {
            return await _context.OrderStates.ToListAsync();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<OrderState>> Get(string id)
        {
            var orderState = await _context.OrderStates.FindAsync(id);

            if (orderState == null)
            {
                return NotFound();
            }

            return orderState;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, OrderStateDTO _orderStateDTO)
        {

            if (id != _orderStateDTO.OrderStateId)
            {
                return BadRequest();
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                //  Set context
                //_context.SetUserID(_context.LoggedInUserId);

                //  POJO code goes here                
                var _item = _mapper.Map<OrderState>(_orderStateDTO);
                _context.OrderStates.Update(_item);
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
        public async Task<ActionResult<OrderState>> Post(OrderStateDTO _orderStateDTO)
        {
            OrderState _item = _mapper.Map<OrderState>(_orderStateDTO);
            _item.OrderStateId = null;      //  Force a new PK to be created
            _context.OrderStates.Add(_item);
            await _context.SaveChangesAsync();

            CreatedAtActionResult ret = CreatedAtAction("Get", new { id = _item.OrderStateId }, _item);
            return Ok(ret);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var orderState = await _context.OrderStates.FindAsync(id);
            if (orderState == null)
            {
                return NotFound();
            }

            _context.OrderStates.Remove(orderState);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Exists(string id)
        {
            return _context.OrderStates.Any(e => e.OrderStateId == id);
        }


    }
}
