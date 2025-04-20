using EnhancementAPI.Models.Repository.UserDetailsRepository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static EnhancementAPI.DBContext.EFCoreInMemory;

namespace EnhancementAPI.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class LoginController : Controller
    {
        private IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }
        
        [HttpPost]
        public IActionResult Login([FromBody] User login)
        {
            // Check for user authentication
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(login);

            if (user != null)
            {
                // Generate Token and Return Both Token and User Role to show dashboard according to user
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new { token = tokenString, Role = user.Role });
            }
            else
            {
                response = Ok(new { errormessage = "Invalid Credentials" });
            }

            return response;
        }
        private string GenerateJSONWebToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),
                new Claim(JwtRegisteredClaimNames.Email, userInfo.Password),
                new Claim("Role", userInfo.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private User AuthenticateUser(User login)
        {
            User user = null;

            
            if (login.Username == login.Username && login.Password == login.Password)
            {
                // It will fetch user details data and will check if user credentials are matching or not.
                UserDetailsRepository repo = new UserDetailsRepository();
               var UserList =  repo.GetUserDetails();
               
                    var getuserData = UserList.Where(a => a.Username == login.Username && a.Password == login.Password).ToList();
                   if(getuserData.Count>0)
                    {
                        user = new User 
                        { Username = getuserData[0].Username, 
                          Password = getuserData[0].Password,
                          Role = getuserData[0].Role
                        };

                        return user;
                    }
                   else
                    {
                    return null;
                    }
                

                
            }
            return user;
        }
    }
}