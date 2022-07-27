using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HandyMan.Data;
using HandyMan.Models;
using AutoMapper;
using HandyMan.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace HandyMan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class CraftController : ControllerBase
    {
        private readonly ICraftRepository _craftRepository;
  

        public CraftController(ICraftRepository craftRepository)
        {
            _craftRepository = craftRepository;
        
        }

        // GET: api/Craft
        [HttpGet]
    
        public async Task<ActionResult<IEnumerable<Craft>>> Get()
        {
            try
            {
                var crafts = await _craftRepository.GetCraftAsync();
             
                return Ok(crafts);
            }
            catch
            {
                return NotFound(new { message = "Empty!" });
            }
        }

        [HttpGet("{id:int}")]
      
        public async Task<ActionResult<Craft>> Get(int id)
        {
            try
            {
                var craft = await _craftRepository.GetCraftByIdAsync(id);
                if (craft == null)
                {
                    return NotFound(new { message = "Craft Is Not Found!" });
                }
                return craft;
            }
            catch
            {
                return NotFound();
            }
        }



        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Put(int id, Craft craft)
        {
            if (id != craft.Craft_ID)
            {
                return NotFound(new { message = "Craft Is Not Found!" });
            }

           
            _craftRepository.EditCraft(craft);
            try
            {
                await _craftRepository.SaveAllAsync();
            }
            catch
            {
                return BadRequest();
            }

            return NoContent();
        }

        // POST: api/Craft
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy="Admin")]
        public async Task<ActionResult<Craft>> Post(Craft craft)
        {
            if (craft == null)
            {
                return NotFound(new { message = "Craft Is Not Found!" });
            }
            if (ModelState.IsValid)
            {
              
                _craftRepository.CreateCraft(craft);
                try
                {
                    await _craftRepository.SaveAllAsync();
                }
                catch
                {
                    return BadRequest(new { Error = "Can't Add This User!" });
                }
                return CreatedAtAction("GetCraft", new { id = craft.Craft_ID }, craft);
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        // DELETE: api/Craft/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _craftRepository.DeleteCraftById(id);
                await _craftRepository.SaveAllAsync();
            }
            catch
            {
                return NotFound(new { message = "Craft Is Not Found!" });
            }

            return NoContent();
        }
    }
}
