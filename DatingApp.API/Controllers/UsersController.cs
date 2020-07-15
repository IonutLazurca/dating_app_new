using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        public UsersController(IDatingRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;

        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repo.GetUsers();

            var userToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            return Ok(userToReturn);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);

            var userToReturn = _mapper.Map<UserForDetailedDto>(user);
            if (userToReturn != null)
            {
                return Ok(userToReturn);
            }

            return BadRequest("User does not exists");

        }

        [HttpPut("{id}")]//used for updating an resource on api    
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto) 
        {
            //check user id from route with user id from token
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();            

            var userFromRepo = await _repo.GetUser(id);

            // var original = userFromRepo.Interests.ToString();
            // var sep1 = " Info added on ";
            // var sep2 = " ";
            // var addon =  userForUpdateDto.Interests.ToString();
            // var dateTime = DateTime.Now.ToShortDateString();
            // var update = string.Concat(original, sep1, dateTime, sep2, addon);
            // userForUpdateDto.Interests = update;                

            _mapper.Map(userForUpdateDto, userFromRepo);

            if (await _repo.SaveAll())
                return NoContent();

            throw new Exception("Updating user {id} failed on save");

            ///extract user from database

            // copy userforeditdto to user object extracted previously from database
            
            //check for errors

            //save new info into database


        }

    }

   
}