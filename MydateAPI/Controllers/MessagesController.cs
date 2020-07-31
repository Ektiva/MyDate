using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using MydateAPI.Data;
using MydateAPI.DTOs;
using MydateAPI.Helpers;
using MydateAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MydateAPI.Repositories.Interfaces;

namespace MydateAPI.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMyDateRepository _repo;
        private readonly IMapper _mapper;
        public MessagesController(IMyDateRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messageFromRepo = await _repo.GetMessage(id);

            if (messageFromRepo == null)
                return NotFound();

            return Ok(messageFromRepo);
        }

        [HttpGet("chat/{id}", Name = "GetChat")]
        public async Task<IActionResult> GetChat(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messageFromRepo = await _repo.GetMessage(id);

            var messageToReturn = _mapper.Map<ChatToReturnDto>(messageFromRepo);
            if (messageFromRepo == null)
                return NotFound();

            return Ok(messageToReturn);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessagesForUser(int userId,
           [FromQuery]MessageParams messageParams)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            messageParams.UserId = userId;

            var messagesFromRepo = await _repo.GetMessagesForUser(messageParams);

            var messages = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromRepo);

            Response.AddPagination(messagesFromRepo.CurrentPage, messagesFromRepo.PageSize,
               messagesFromRepo.TotalCount, messagesFromRepo.TotalPages);

            return Ok(messages);
        }

        //Conversation between 2 users
        [HttpGet("thread/{recipientId}")]
        public async Task<IActionResult> GetMessageThread(int userId, int recipientId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messagesFromRepo = await _repo.GetMessageThread(userId, recipientId);

            var messageThread = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromRepo);

            return Ok(messageThread);
        }

        //Conversation between 2 users
        [HttpGet("thread/group/{recipientId}")]
        public async Task<IActionResult> GetMessageThreadGroup(int userId, int recipientId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messagesFromRepo = await _repo.GetMessageThread(userId, recipientId);

            var chatThread = new ChatDto();
            chatThread.Id = ""+userId + ""+recipientId;
            chatThread.Dialog = _mapper.Map<IEnumerable<ChatToReturnDto>>(messagesFromRepo);
            //var messageThread = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromRepo);

            return Ok(chatThread);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId, MessageForCreationDto messageForCreationDto)
        {
           var sender = await _repo.GetUser(userId);

            if (sender.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            messageForCreationDto.SenderId = userId;

            var recipient = await _repo.GetUser(messageForCreationDto.RecipientId);

            if (recipient == null)
                return BadRequest("Could not find user");

            var message = _mapper.Map<Message>(messageForCreationDto);

            _repo.Add(message);

            if (await _repo.SaveAll())
            {
                //var messageToReturn = _mapper.Map<MessageToReturnDto>(message);
                //return CreatedAtRoute("GetMessage",
                //    new { userId, id = message.Id }, messageToReturn);
                var messageToReturn = _mapper.Map<ChatToReturnDto>(message);
                return CreatedAtRoute("GetChat",
                    new { userId, id = message.Id }, messageToReturn);
            }

            throw new Exception("Creating the message failed on save");
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteMessage(int id, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messageFromRepo = await _repo.GetMessage(id);

            if (messageFromRepo.SenderId == userId)
                messageFromRepo.SenderDeleted = true;

            if (messageFromRepo.RecipientId == userId)
                messageFromRepo.RecipientDeleted = true;

            if (messageFromRepo.SenderDeleted && messageFromRepo.RecipientDeleted)
                _repo.Delete(messageFromRepo);

            if (await _repo.SaveAll())
                return NoContent();

            throw new Exception("Error deleting the message");
        }

        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkMessageAsRead(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var message = await _repo.GetMessage(id);

            if (message.RecipientId != userId)
                return Unauthorized();

            message.IsRead = true;
            message.DateRead = DateTime.Now;

            await _repo.SaveAll();

            return NoContent();
        }

        [HttpPost("{senderId}/reads")]
        public async Task<IActionResult> MarkMessagesAsRead(int userId, int senderId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messages = await _repo.GetMessages(userId, senderId);

            foreach(var message in messages)
            {
                if (message.IsRead == false)
                {
                    message.IsRead = true;
                    message.DateRead = DateTime.Now;
                }
            }

            await _repo.SaveAll();

            return NoContent();
        }
    }
}