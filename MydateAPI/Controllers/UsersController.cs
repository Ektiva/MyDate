using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MydateAPI.DTOs;
using MydateAPI.Repositories.Interfaces;

namespace MydateAPI.Controllers
{
    //[ServiceFilter(typeof(LogUserActivity))]
    //To make sure that any user who request this controller have authorization
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMyDateRepository _repo;
        private readonly IMapper _mapper;
        public UsersController(IMyDateRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }


        //Get User and Users without Mapping
        //[HttpGet]
        //public async Task<IActionResult> GetUsers()
        //{
        //    var users = await _repo.GetUsers();


        //    return Ok(users);
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetUser(int id)
        //{
        //    var user = await _repo.GetUser(id);

        //    return Ok(user);
        //}

        //Get User and Users with Automapper
        [HttpGet("{id}"/*, Name = "GetUser"*/)]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);
            var userToReturn = _mapper.Map<UserForDetailedDto>(user);

            return Ok(userToReturn);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repo.GetUsers();

            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            return Ok(usersToReturn);
        }

        ///For later
        //[HttpGet]
        //public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        //{
        //    var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        //    var userFromRepo = await _repo.GetUser(currentUserId);

        //    userParams.UserId = currentUserId;

        //    if (string.IsNullOrEmpty(userParams.Gender))
        //    {
        //        userParams.Gender = userFromRepo.Gender == "male" ? "female" : "male";
        //    }

        //    var users = await _repo.GetUsers(userParams);

        //    var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

        //    Response.AddPagination(users.CurrentPage, users.PageSize,
        //         users.TotalCount, users.TotalPages);

        //    return Ok(usersToReturn);
        //}



        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        //{
        //    if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
        //        return Unauthorized();

        //    var userFromRepo = await _repo.GetUser(id);

        //    _mapper.Map(userForUpdateDto, userFromRepo);

        //    if (await _repo.SaveAll())
        //        return NoContent();

        //    throw new Exception($"Updating user {id} failed on save");
        //}

        //[HttpPost("{id}/like/{recipientId}")]
        //public async Task<IActionResult> LikeUser(int id, int recipientId)
        //{
        //    if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
        //        return Unauthorized();

        //    var like = await _repo.GetLike(id, recipientId);

        //    if (like != null)
        //        return BadRequest("You already like this user");

        //    if (await _repo.GetUser(recipientId) == null)
        //        return NotFound();

        //    like = new Like
        //    {
        //        LikerId = id,
        //        LikeeId = recipientId
        //    };

        //    _repo.Add<Like>(like);

        //    if (await _repo.SaveAll())
        //        return Ok();

        //    return BadRequest("Failed to like user");
        //}
    }
}