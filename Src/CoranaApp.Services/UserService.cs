//using CoronaApp.Api;
//using CoronaApp.Services.Models;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.Tokens;
//using System;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Security.Claims;
//using System.Text;

//namespace CoronaApp.Services
//{
//    public class UserService : IUserService
//    {
//        public ICollection<Location> GetAll()
//        {
//            var locationService = new LocationService();
//            return locationService.Get();
//        }
      
//        private List<User> _users = new List<User>
//        {
//            new User { Id = 1, Username = "test", Password = "test" }
//        };
//        public UserService(IConfiguration configuration)
//        {
//            _config = configuration;
//        }
//        private readonly IConfiguration _config;

//        public User Authenticate(string userName, string password)
//        {
//            var user = _users.SingleOrDefault(x => x.Username == userName && x.Password == password);

//            // return null if user not found
//            if (user == null)
//                return null;

//            // authentication successful so generate jwt token
//            var tokenHandler = new JwtSecurityTokenHandler();
//            var key = Encoding.ASCII.GetBytes(_config.GetSection("AppSettings").GetSection("Secret").Value);
//            var tokenDescriptor = new SecurityTokenDescriptor
//            {
//                Subject = new ClaimsIdentity(new Claim[]
//                {
//                    new Claim(ClaimTypes.Name, user.Password)
//                }),
//                Expires = DateTime.UtcNow.AddHours(1),
//                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
//            };
//            var token = tokenHandler.CreateToken(tokenDescriptor);
//            user.Token = tokenHandler.WriteToken(token);

//            return user;
//        }      
//    }
//}

