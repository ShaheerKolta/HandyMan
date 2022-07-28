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
using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using HandyMan.Dtos;

namespace HandyMan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class HandymanController : ControllerBase
    {
        private readonly IHandymanRepository handymanRepository;
        private readonly IMapper _mapper;

        public HandymanController(IHandymanRepository _handymanRepository, IMapper mapper)
        {
            handymanRepository = _handymanRepository;
            _mapper = mapper;
        }

        [HttpGet("All")]
        [Authorize(Policy = "Admin")]

        public async Task<ActionResult<IEnumerable<HandymanDto>>> GetAllHandyman()
        {

            try
            {
                var handymen = await handymanRepository.GetHandyManAsync();
                var res = _mapper.Map<IEnumerable<HandymanDto>>(handymen);
                return Ok(res);
            }
            catch
            {
                return NotFound(new { message = "Empty!" });
            }
        }

        // GET: api/Handyman
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<HandymanDto>>> GetHandyman()//Verified only
        {

            try
            {
                var handymen = await handymanRepository.GetVerifiedHandyManAsync();
                var res = _mapper.Map<IEnumerable<HandymanDto>>(handymen);
                return Ok(res);
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

        // Problem -> Need to be accessed using client to activate the Request Functionalities 
        // Suggessted Solution is to create a special GetHandmanbyIDRequest 
        public async Task<ActionResult<HandymanDto>> GetHandyman(int id, [FromHeader] string Authorization)
        {
            JwtSecurityToken t = (JwtSecurityToken)new JwtSecurityTokenHandler().ReadToken(Authorization.Substring(7));
            var x = t.Claims.ToList();
            
            if (x[0].Value !=id.ToString())
            {
                return Unauthorized();
            }
            try
            {
                
                var handyman = await handymanRepository.GetHandymanByIdAsync(id);
                handymanRepository.CalculateHandymanRate(handyman);
                if (handyman == null)
                {
                    return NotFound(new { message = "Handyman Is Not Found!" });
                }
                return _mapper.Map<HandymanDto>(handyman);

            }
            catch
            {
                return NotFound();
            }
        }

        // PUT: api/Handyman/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = "Handyman")]
        public async Task<IActionResult> EditHandyman(int id, HandymanDto handymandto)
        {
            if (id != handymandto.Handyman_SSN)
            {
                return BadRequest();
            }
            var handyman = _mapper.Map<Handyman>(handymandto);
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
        [HttpPost("/api/Register/Handyman")]
        [AllowAnonymous]
        public async Task<ActionResult<HandymanDto>> PostHandyman(HandymanDto handymandto)
        {
            if (handymandto == null)
            {
                return Problem("Handyman is Empty");
            }
            var handyman = _mapper.Map<Handyman>(handymandto);
            handymanRepository.CreateHandyman(handyman);
            try
            {
                await handymanRepository.SaveAllAsync();
            }
            catch 
            {
                return BadRequest(new { message= "Can't Save!"});
            }

            return CreatedAtAction("GetHandyman", new { id = handymandto.Handyman_SSN }, handymandto);
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
        [HttpGet("status/{id}")]
        [Authorize(Policy ="Handyman")]
        public async Task<IActionResult> ToggleHandymanStatus(int id)
        {
            Handyman handyman = await handymanRepository.GetHandymanByIdAsync(id);
            handyman.Open_For_Work = !handyman.Open_For_Work;
            return Ok();
        }


    }
}