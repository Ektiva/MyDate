using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MydateAPI.Controllers;
using MydateAPI.DTOs;
using MydateAPI.Models;
using MydateAPI.Repositories;

namespace MydateAPI.Controllers
{
    //[EnableCors("DefaultPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public AuthController(IAuthRepository repo, IConfiguration config, IMapper mapper)
        {
            _config = config;
            _repo = repo;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            //Validate the request

            //Convert username to lower case to avoid duplicate username
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await _repo.UserExists(userForRegisterDto.Username))
                return BadRequest("Username already exist");

            //var userToCreate = new User
            //{
            //    Username = userForRegisterDto.Username
            //};

            var userToCreate = _mapper.Map<User>(userForRegisterDto);

            var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);

            var userToReturn = _mapper.Map<UserForDetailedDto>(createdUser);

            //return StatusCode(201);
            return CreatedAtRoute("GetUser", new { controller = "Users", id = createdUser.Id }, userToReturn);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDTO)
        {
            //throw new Exception("Computer says No!");

            var userFromRepo = await _repo.Login(userForLoginDTO.Username.ToLower(), userForLoginDTO.Password);

            if (userFromRepo == null)
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

            //Created a security Key for the signin credential and encrypted the key
            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));

            //Sign in credential
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            //Security token descriptor to contain claim, expiry date of token and signin  credential
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var user = _mapper.Map<UserForListDto>(userFromRepo);

            //return the token as an object to the client
            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                user
            });

        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _repo.EmailExists(email);
        }

    }
}