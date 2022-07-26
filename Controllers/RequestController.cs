using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HandyMan.Dtos;
using HandyMan.Interfaces;
using HandyMan.Models;
using Microsoft.AspNetCore.Authorization;

namespace HandyMan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IMapper _mapper;

        public RequestController(IRequestRepository requestRepository, IMapper mapper)
        {
            _requestRepository = requestRepository;
            _mapper = mapper;
        }

        // GET: api/Request
        [HttpGet]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<IEnumerable<RequestDto>>> GetRequests()
        {
            try
            {
                var requests = await _requestRepository.GetRequestsAsync();
                var requestsToReturn = _mapper.Map<IEnumerable<RequestDto>>(requests);
                return Ok(requestsToReturn);
            }
            catch
            {
                return NotFound(new { message = "Empty!" });
            }
        }
        




        // GET: api/Request/5
        [HttpGet("{id:int}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<RequestDto>> GetRequest(int id)
        {
            try
            {
                var request = await _requestRepository.GetRequestByIdAsync(id);
                if (request == null)
                {
                    return NotFound(new { message = "Request Is Not Found!" });
                }
                return _mapper.Map<RequestDto>(request);
            }
            catch
            {
                return NotFound();
            }
        }


        [HttpGet("client/{id}")]
        [Authorize(Policy ="Client")]
        public async Task<ActionResult<IEnumerable<RequestDto>>> GetRequestsByClientId(int id)
        {
            try
            {
                var requests = await _requestRepository.GetRequestsByClientIdAsync(id);
                var requestsToReturn = _mapper.Map<IEnumerable<RequestDto>>(requests);
                return Ok(requestsToReturn);
            }
            catch
            {
                return NotFound(new { message = "Empty!" });
            }
        }


        [HttpGet("handyman/{handymanSsn}")]
        [Authorize(Policy = "Handyman")]
        //function to get all incoming requests of a handyman
        public async Task<ActionResult<IEnumerable<RequestDto>>> GetRequestsByHandymanSsn(int handymanSsn)
        {
            try
            {
                var requests = await _requestRepository.GetRequestsByHandymanSsnAsync(handymanSsn);
                var requestsToReturn = _mapper.Map<IEnumerable<RequestDto>>(requests);
                return Ok(requestsToReturn);
            }
            catch
            {
                return NotFound(new { message = "Empty!" });
            }
        }

        // PUT: api/Request/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> EditRequest(int id, RequestDto requestDto)
        {
            if (id != requestDto.Request_ID)
            {
                return NotFound(new { message = "Request Is Not Found!" });
            }

            var request = _mapper.Map<Request>(requestDto);
            _requestRepository.EditRequest(request);
            try
            {
                await _requestRepository.SaveAllAsync();
            }
            catch
            {
                return BadRequest();
            }

            return NoContent();
        }

        // POST: api/Request
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = "Client")]
        public async Task<ActionResult<RequestDto>> PostRequest(RequestDto requestDto)
        {
            
            
            if (requestDto == null)
            {
                return NotFound(new { message = "Request Is Not Found!" });
            }
            if (ModelState.IsValid)
            {
                var request = _mapper.Map<Request>(requestDto);
                _requestRepository.CreateRequest(request);
                try
                {
                    await _requestRepository.SaveAllAsync();
                }
                catch
                {
                    return BadRequest(new { Error = "Can't Add This Request!" });
                }
                return CreatedAtAction("GetRequest", new { id = requestDto.Request_ID }, requestDto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/Request/5
        [HttpDelete("{id}")]
        // Admin or Handyman
        [Authorize(Policy = "Handyman")] // If Handyman -> Decline -> delete ? or cancelled ??
        public async Task<IActionResult> DeleteRequest(int id)
        {
            try
            {
               // _requestRepository.EditRequest(_mapper.Map<Request>(id));
                _requestRepository.DeleteRequestById(id);
                await _requestRepository.SaveAllAsync();
            }
            catch
            {
                return NotFound(new { message = "Request Is Not Found!" });
            }

            return NoContent();
        }
    }
}
