using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HandyMan.Data;
using HandyMan.Models;
using HandyMan.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace HandyMan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy ="Admin")]
    public class HandymanController : ControllerBase
    {
        private readonly IHandymanRepository handymanRepository;

        public HandymanController(IHandymanRepository _handymanRepository)
        {
            handymanRepository = _handymanRepository;
        }

        // GET: api/Handyman
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Handyman>>> GetHandyman()
        {
            try
            {
                return Ok(await handymanRepository.GetHandyManAsync());
            }
            catch
            {
                return NotFound(new { message = "Empty!" });
            }
        }

        // GET: api/Handyman/5
        //is the same attributes going to show to user and handyman himself ??
        [HttpGet("{id}")]
        [Authorize(Policy = "Handyman")]
        public async Task<ActionResult<Handyman>> GetHandyman(int id)
        {
            try
            {
                Handyman handyman = await handymanRepository.GetHandymanByIdAsync(id);
                if (handyman == null)
                {
                    return NotFound(new { message = "Client Is Not Found!" });
                }
                return handyman;
            }
            catch
            {
                return NotFound();
            }
        }

        // PUT: api/Handyman/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy ="Handyman")]
        public async Task<IActionResult> EditHandyman(int id, Handyman handyman)
        {
            if (id != handyman.Handyman_SSN)
            {
                return BadRequest();
            }

            handymanRepository.EditHandyman(handyman);

            try
            {
                await handymanRepository.SaveAllAsync();
            }
            catch 
            {
                
                    return NotFound();
                
            }

            return NoContent();
        }

        // POST: api/Handyman
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<Handyman>> PostHandyman(Handyman handyman)
        {
          if (handyman == null)
          {
              return Problem("Handyman is Empty");
          }
            handymanRepository.CreateHandyman(handyman);
            try
            {
                await handymanRepository.SaveAllAsync();
            }
            catch (DbUpdateException)
            {
                if (handymanRepository.GetHandymanByIdAsync(handyman.Handyman_SSN) !=null)
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetHandyman", new { id = handyman.Handyman_SSN }, handyman);
        }

        // DELETE: api/Handyman/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "Handyman")]
        public async Task<IActionResult> DeleteHandyman(int id)
        {
            
            var handyman = await handymanRepository.GetHandymanByIdAsync(id);
            if (handyman == null)
            {
                return NotFound();
            }

            try
            {
                handymanRepository.DeleteHandymanById(id);
                await handymanRepository.SaveAllAsync();
            }
            catch
            {
                return NotFound(new { message = "Client Is Not Found!" });
            }

            return NoContent();
        }

       
    }
}
