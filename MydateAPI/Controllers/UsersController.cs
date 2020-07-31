using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using MydateAPI.Data;
using MydateAPI.DTOs;
using MydateAPI.Helpers;
using MydateAPI.Models;
using MydateAPI.Repositories.Interfaces;

namespace MydateAPI.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    //To make sure that any user who request this controller have authorization
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMyDateRepository _repo;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public UsersController(IMyDateRepository repo, IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _repo = repo;
            _context = context;
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
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);
            var userToReturn = _mapper.Map<UserForDetailedDto>(user);

            return Ok(userToReturn);
        }

        //[HttpGet]
        //public async Task<IActionResult> GetUsers()
        //{
        //    var users = await _repo.GetUsers();

        //    var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

        //    return Ok(usersToReturn);
        //}

        ///For later
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var userFromRepo = await _repo.GetUser(currentUserId);

            userParams.UserId = currentUserId;

            if (string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = userFromRepo.Gender == "male" ? "female" : "male";
            }

            var users = await _repo.GetUsers(userParams);

            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            Response.AddPagination(users.CurrentPage, users.PageSize,
                 users.TotalCount, users.TotalPages);

            return Ok(usersToReturn);
        }

        ///For later
        [HttpGet("contacts")]
        public async Task<IActionResult> GetContacts([FromQuery] UserParams userParams)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var userFromRepo = await _repo.GetUser(currentUserId);

            userParams.UserId = currentUserId;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userParams.UserId);

            userParams.Gender = "male";
            var usersMale = await _repo.GetUserss(userParams);
            var usersMalesSave = usersMale.ToList();

            userParams.Gender = "female";
            var usersFemale = await _repo.GetUserss(userParams);
            var usersFemalesSave = usersFemale.ToList();

            var users = usersMalesSave.Concat(usersFemalesSave);

            var likers = user.Likers.Where(u => u.LikeeId == userParams.UserId).Select(i => i.LikerId);
            var likees = user.Likees.Where(u => u.LikerId == userParams.UserId).Select(i => i.LikeeId);

            var recipient = await _context.Messages.Where(m => m.RecipientId == userParams.UserId)
                    .Select(i => i.SenderId)
                    .ToListAsync();
            var recipientSave = recipient;

            var sender = await _context.Messages.Where(m => m.SenderId == userParams.UserId)
                    .Select(i => i.RecipientId)
                    .ToListAsync();

            var senderSave = sender;

            //var users1 = new PagedList<User>(List < T > items, int count, int pageNumber, int pageSize);

            var userss = users.Where(u => likers.Contains(u.Id) || likees.Contains(u.Id) || recipientSave.Contains(u.Id) || senderSave.Contains(u.Id));

            var usersToReturn = _mapper.Map<IEnumerable<UserForContactDto>>(userss);

            //Response.AddPagination(users.CurrentPage, users.PageSize,
            //     users.TotalCount, users.TotalPages);

            return Ok(usersToReturn);
        }

        [HttpGet("likes")]
        public async Task<IActionResult> GetLikes([FromQuery] UserParams userParams)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var userFromRepo = await _repo.GetUser(currentUserId);

            userParams.UserId = currentUserId;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userParams.UserId);

            //var users = new List<User>();
            userParams.Gender = "male";
            var usersMale = await _repo.GetUserss(userParams);
            var usersMalesSave = usersMale.ToList();

            userParams.Gender = "female";
            var usersFemale = await _repo.GetUserss(userParams);
            var usersFemalesSave = usersFemale.ToList();

            var users = usersMalesSave.Concat(usersFemalesSave);

            var likers = user.Likers.Where(u => u.LikeeId == userParams.UserId).Select(i => i.LikerId);
            var likees = user.Likees.Where(u => u.LikerId == userParams.UserId).Select(i => i.LikeeId);

            //var users1 = new PagedList<User>(List < T > items, int count, int pageNumber, int pageSize);

            var userss = users.Where(u => likers.Contains(u.Id) || likees.Contains(u.Id));

            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(userss);

            //Response.AddPagination(users.CurrentPage, users.PageSize,
            //     users.TotalCount, users.TotalPages);

            return Ok(usersToReturn);
        }


        [HttpGet("messages")]
        public async Task<IActionResult> GetMessages([FromQuery] UserParams userParams)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var userFromRepo = await _repo.GetUser(currentUserId);

            userParams.UserId = currentUserId;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userParams.UserId);

            userParams.Gender = "male";
            var usersMale = await _repo.GetUserss(userParams);
            var usersMalesSave = usersMale.ToList();

            userParams.Gender = "female";
            var usersFemale = await _repo.GetUserss(userParams);
            var usersFemalesSave = usersFemale.ToList();

            var users = usersMalesSave.Concat(usersFemalesSave);

            var recipient = await _context.Messages.Where(m => m.RecipientId == userParams.UserId)
                    .Select(i => i.SenderId)
                    .ToListAsync();
            var recipientSave = recipient;

            var sender = await _context.Messages.Where(m => m.SenderId == userParams.UserId)
                    .Select(i => i.RecipientId)
                    .ToListAsync();
            var senderSave = sender;

            var userss = users.Where(u => recipientSave.Contains(u.Id) || senderSave.Contains(u.Id));

            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(userss);

            //Response.AddPagination(users.CurrentPage, users.PageSize,
            //     users.TotalCount, users.TotalPages);

            return Ok(usersToReturn);
        }

        //All Conversations of 1 user with the user information
        [HttpGet("thread/{userId}")]
        public async Task<IActionResult> GetAllMessageThread(int userId)
        {

            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var baseChatId = ""+userId;

            var recipient = await _context.Messages.Where(m => m.RecipientId == userId)
                   .Select(i => i.SenderId)
                    .Distinct()
                   .ToListAsync();
            var recipientSave = recipient;

            var sender = await _context.Messages.Where(m => m.SenderId == userId)
                    .Select(i => i.RecipientId)
                    .Distinct()
                    .ToListAsync();
            var senderSave = sender;

            var usersIdWithMessage = await _context.Users.Where(u => recipientSave.Contains(u.Id) || senderSave.Contains(u.Id))
                .Select(i => i.Id)
                 .Distinct()
                .ToListAsync();

            var allMessageThreads = new List<ChatDto>();
            foreach(var id in usersIdWithMessage)
            {
                var chatId = baseChatId; chatId += id;

                var messagesFromRepo = await _repo.GetMessageThread(userId, id);
                var messageThread = _mapper.Map<IEnumerable<ChatToReturnDto>>(messagesFromRepo);

                ChatDto currChat = new ChatDto();
                currChat.Id = chatId; currChat.Dialog = messageThread;
                allMessageThreads.Add(currChat);
            }

            return Ok(allMessageThreads);
        }

        //All Conversations of 1 user with the user information
        [HttpGet("thread/userinfo/{userId}")]
        public async Task<IActionResult> GetAllMessageUserThread(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var userToReturn = new UserForChatDto();
            userToReturn.Id = currentUser.Id ;
            userToReturn.Mood = currentUser.Mood ;
            userToReturn.Name = currentUser.Username;
            userToReturn.Status = currentUser.Status;
            userToReturn.Avatar = currentUser.Avatar;

            var baseChatId = "" + userId;

            var recipient = await _context.Messages.Where(m => m.RecipientId == userId)
                   .Select(i => i.SenderId)
                    .Distinct()
                   .ToListAsync();
            var recipientSave = recipient;

            var sender = await _context.Messages.Where(m => m.SenderId == userId)
                    .Select(i => i.RecipientId)
                    .Distinct()
                    .ToListAsync();
            var senderSave = sender;

            var usersIdWithMessage = await _context.Users.Where(u => recipientSave.Contains(u.Id) || senderSave.Contains(u.Id))
                .Select(i => i.Id)
                 .Distinct()
                .ToListAsync();

            var allChatList = new List<ChatListDto>();
            foreach (var id in usersIdWithMessage)
            {
                var messagesFromRepo = await _repo.GetMessageThread(userId, id);

                var chatListDto = new ChatListDto();
                chatListDto.Id = baseChatId + id;
                chatListDto.contactId = id;
                //var MinDate = DateTime.MinValue;

                var messageThread = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromRepo);
                var firstMess = messageThread.Last();
                if (firstMess.RecipientId != userId) 
                {
                    chatListDto.Name = firstMess.RecipientKnownAs;
                }
                else
                {
                    chatListDto.Name = firstMess.SenderKnownAs;
                }

                chatListDto.Unread = 0;
                foreach (var mess in messageThread)
                {
                    if ( mess.RecipientId == userId && !mess.IsRead) { chatListDto.Unread++; }
                }
                chatListDto.LastMessage = firstMess.Content;
                chatListDto.LastMessageTime = firstMess.MessageSent;

                allChatList.Add(chatListDto);
            }

            userToReturn.ChatList = allChatList;

            var userToReturnList = new List<UserForChatDto>();
            userToReturnList.Add(userToReturn);

            return Ok(userToReturnList);
        }

        //All Conversations of 1 user with the user information
        [HttpGet("userinfo/{userId}")]
        public async Task<IActionResult> GetUserInfo(int userId)
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            //var userToReturn = _mapper.Map<UserForReturnDto>(currentUser);
            var age = DateTime.Today.Year - currentUser.DateOfBirth.Year;
            if (currentUser.DateOfBirth.AddYears(age) > DateTime.Today)
                age--;
            var userToReturn = new UserForReturnDto()
            {
                Id = currentUser.Id,
                Username = currentUser.Username,
                Gender = currentUser.Gender,
                DateOfBirth = currentUser.DateOfBirth,
                Age = age,
                KnownAs = currentUser.KnownAs,
                Created = currentUser.Created,
                LastActive = currentUser.LastActive,
                Introduction = currentUser.Introduction,
                LookingFor = currentUser.LookingFor,
                Interests = currentUser.Interests,
                City = currentUser.City,
                Country = currentUser.Country,
                Lastname = currentUser.Lastname,
                Avatar = currentUser.Avatar,
                Company = currentUser.Company,
                Jobtitle = currentUser.Jobtitle,
                Email = currentUser.Email,
                Phone = currentUser.Phone,
                Address = currentUser.Address,
                Notes = currentUser.Notes,
                Status = currentUser.Status,
                Mood = currentUser.Mood
            };
            return Ok(userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(id);

            _mapper.Map(userForUpdateDto, userFromRepo);

            if (await _repo.SaveAll())
                return NoContent();

            throw new Exception($"Updating user {id} failed on save");
        }

        [HttpPost("{id}/like/{recipientId}")]
        public async Task<IActionResult> LikeUser(int id, int recipientId)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var like = await _repo.GetLike(id, recipientId);

            if (like != null)
            {
                _repo.Delete<Like>(like);
                if (await _repo.SaveAll())
                    return Ok("You DisLike this User");
                return BadRequest("Failed to DisLike user");
            }
            if (await _repo.GetUser(recipientId) == null)
                return NotFound();

            like = new Like
            {
                LikerId = id,
                LikeeId = recipientId
            };

            _repo.Add<Like>(like);
            if (await _repo.SaveAll())
                return Ok("You successfully Like this User");

            /*if (like != null)
                return BadRequest("You already like this user");

            if (await _repo.GetUser(recipientId) == null)
                return NotFound();

            like = new Like
            {
                LikerId = id,
                LikeeId = recipientId
            };

            _repo.Add<Like>(like);

            if (await _repo.SaveAll())
                return Ok();*/

            return BadRequest("Failed to like user");
        }

        [HttpGet("{id}/isLike/{recipientId}")]
        public async Task<IActionResult> UseIsLike(int id, int recipientId)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var like = await _repo.GetLike(id, recipientId);

            if (like != null)
            {
                return Ok(true);
            }
            
            return Ok(false);
        }

        [HttpDelete("{id}/like/{recipientId}")]
        public async Task<IActionResult> CancelLikeUser(int id, int recipientId)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var like = await _repo.GetLike(id, recipientId);

            if (like == null)
            {
                return BadRequest("You didn't like this user already");           
            }

            if (await _repo.GetUser(recipientId) == null)
                return NotFound();

            _repo.Delete<Like>(like);

            if (await _repo.SaveAll())
                return Ok();

            return BadRequest("Failed to Dislike user");
        }
    }
}