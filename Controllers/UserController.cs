using JWTAuthentication.Models;
using JWTAuthentication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthentication.Controllers
{
    [Route("Users")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private IUserRepository userRepository;
        public UserController(IUserRepository repository)
        {
            userRepository = repository;
        }


        [HttpPost] 
        public User Add(User userData)
        { 
            if (userData == null)
                throw new Exception("Please provide data.");
            return userRepository.Add(userData: userData);
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public string Login([FromBody] User request)
        {
            if (request == null)
                throw new Exception("Please provide credentials.");
            if (string.IsNullOrEmpty(request.username))
                throw new Exception("Please provide username.");
            if (string.IsNullOrEmpty(request.password))
                throw new Exception("Please provide password.");

            var existUser = userRepository.Get(username: request.username);
            if (existUser != null)
            {
                if (existUser.password != request.password)
                    throw new Exception("Password is incorrect.");
                return userRepository.GenerateJSONWebToken(userInfo: request);
            }
            else
                throw new Exception("Can't find user with username");

        }


    }
}
