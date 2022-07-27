﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using HandyMan.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HandyMan.Interfaces;
using HandyMan.Models;
using Microsoft.AspNetCore.Authorization;

namespace HandyMan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

        public ClientController(IClientRepository clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        // GET: api/Client
        [HttpGet]
        [Authorize(Policy ="Admin")]
        public async Task<ActionResult<IEnumerable<ClientDto>>> GetClients()
        {
            try
            {
                var clients = await _clientRepository.GetClientsAsync();
                var clientsToReturn = _mapper.Map<IEnumerable<ClientDto>>(clients);
                return Ok(clientsToReturn);
            }
            catch
            {
                return NotFound(new { message = "Empty!" });
            }
        }


        // to be Checked With Amal Tommorow 
        // GET: api/Client
        [HttpGet("FindHandymen")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<HandymanDto>>> GetHandymen()
        {
            try
            {
                var handymen = await _clientRepository.GetHandymenAsync();
                var handymenToReturn = _mapper.Map<IEnumerable<HandymanDto>>(handymen);
                return Ok(handymenToReturn);
            }
            catch
            {
                return NotFound(new { message = "Empty!" });
            }
        }

        // GET: api/Client/5
        [HttpGet("{id:int}")]
        [Authorize(Policy ="Client")]

        // Problem when be called from The frontend (Request Operation)
        // Suggested Solution -> New GetClientForRequest Function with Handyman Authorization  
        // must have Handyman ID , (Request ID , Client ID) --> Comes from the Front 
        public async Task<ActionResult<ClientDto>> GetClient(int id, [FromHeader] string Authorization)
        {
            JwtSecurityToken t = (JwtSecurityToken)new JwtSecurityTokenHandler().ReadToken(Authorization.Substring(7));
            var x = t.Claims.ToList();
            

            var c = x[0];
            if (x[0].Value != id.ToString())
            {
                return Unauthorized();
            }
            try
            {
                var client = await _clientRepository.GetClientByIdAsync(id);
                if (client == null)
                {
                    return NotFound(new { message = "Client Is Not Found!" });
                }
                return _mapper.Map<ClientDto>(client);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet("{email}")]
        
        public async Task<ActionResult<ClientDto>> GetClientByEmail(string email)
        {
            try
            {
                Client client = await _clientRepository.GetClientByEmail(email);
                if (client == null)
                {
                    return NotFound(new { message = "Client Is Not Found!" });
                }
                return _mapper.Map<ClientDto>(client);
            }
            catch
            {
                return NotFound();
            }
        }

        // PUT: api/Client/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = "Client")]
        public async Task<IActionResult> EditClient(int id, ClientDto clientDto)
        {
            if (id != clientDto.Client_ID)
            {
                return NotFound(new { message = "Client Is Not Found!" });
            }

            var client = _mapper.Map<Client>(clientDto);
            _clientRepository.EditClient(client);
            try
            {
                await _clientRepository.SaveAllAsync();
            }
            catch
            {
                return BadRequest();
            }

            return NoContent();
        }

        // POST: api/Client
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("/api/Register/Client")]
        [AllowAnonymous]
        public async Task<ActionResult<ClientDto>> PostClient(ClientDto clientDto)
        {
            if (clientDto == null)
            {
                return NotFound(new { message = "Client Is Not Found!" });
            }
            if (ModelState.IsValid)
            {
                var client = _mapper.Map<Client>(clientDto);
                _clientRepository.CreateClient(client);
                try
                {
                    await _clientRepository.SaveAllAsync();
                }
                catch
                {
                    return BadRequest(new {Error = "Can't Add This User!" });
                }
                return CreatedAtAction("GetClient", new { id = clientDto.Client_ID }, clientDto);
            }
            else
            {
                return BadRequest(ModelState);
            }
            
        }

        // DELETE: api/Client/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "Client")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            try
            {
                _clientRepository.DeleteClientById(id);
                await _clientRepository.SaveAllAsync();
            }
            catch
            {
                return NotFound(new { message = "Client Is Not Found!" });
            }

            return NoContent();
        }
    }
}
