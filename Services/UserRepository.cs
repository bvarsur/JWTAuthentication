using JWTAuthentication.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthentication.Services
{
    public class UserRepository : IUserRepository
    {
        private BookDbContext db;
        private IConfiguration config;
        public UserRepository(BookDbContext context, IConfiguration configuration)
        {
            db = context;
            config = configuration;
        }

        public User Get(string username)
        {
            return db.Users.Where(x => x.username == username).FirstOrDefault();
        }

        public User Add(User userData)
        {
            var existUser = db.Users.Where(x => x.username == userData.username).FirstOrDefault();
            if (existUser != null)
                return existUser;

            db.Users.Add(userData);
            db.Entry(userData).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            db.SaveChanges();
            return userData;
        }

        public string GenerateJSONWebToken(User userInfo)
        {
            var issuer = config.GetSection("JWTToken:Issuer").Get<string>();
            var key = Encoding.ASCII.GetBytes(config.GetSection("JWTToken:Key").Get<string>());

            //  var securityKey = new SymmetricSecurityKey(key);
            var securityHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userInfo.username)
                }),
                Expires = DateTime.Now.AddMinutes(20),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = securityHandler.CreateToken(tokenDescriptor);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public interface IUserRepository
    {
        User Add(User userData);
        User Get(string username);
        string GenerateJSONWebToken(User userInfo);
    }
}
