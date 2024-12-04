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
    public class OrdersLineController : ControllerBase, iController<OrdersLine, OrdersLineDTO>
    {
        private readonly WebRestOracleContext _context;
        private readonly IMapper _mapper;

        public OrdersLineController(WebRestOracleContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
           // _context.LoggedInUserId = "XYZ";
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrdersLine>>> Get()
        {
            return await _context.OrdersLines.ToListAsync();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<OrdersLine>> Get(string id)
        {
            var ordersLine = await _context.OrdersLines.FindAsync(id);

            if (ordersLine == null)
            {
                return NotFound();
            }

            return ordersLine;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, OrdersLineDTO _ordersLineDTO)
        {

            if (id != _ordersLineDTO.OrdersLineId)
            {
                return BadRequest();
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                //  Set context
                //_context.SetUserID(_context.LoggedInUserId);

                //  POJO code goes here                
                var _item = _mapper.Map<OrdersLine>(_ordersLineDTO);
                _context.OrdersLines.Update(_item);
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
        public async Task<ActionResult<OrdersLine>> Post(OrdersLineDTO _ordersLineDTO)
        {
            OrdersLine _item = _mapper.Map<OrdersLine>(_ordersLineDTO);
            _item.OrdersLineId = null;      //  Force a new PK to be created
            _context.OrdersLines.Add(_item);
            await _context.SaveChangesAsync();

            CreatedAtActionResult ret = CreatedAtAction("Get", new { id = _item.OrdersLineId }, _item);
            return Ok(ret);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var ordersLine = await _context.OrdersLines.FindAsync(id);
            if (ordersLine == null)
            {
                return NotFound();
            }

            _context.OrdersLines.Remove(ordersLine);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Exists(string id)
        {
            return _context.OrdersLines.Any(e => e.OrdersLineId == id);
        }


    }
}
