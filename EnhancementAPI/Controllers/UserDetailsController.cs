using EnhancementAPI.Models;
using EnhancementAPI.Models.Repository.UserDetailsRepository;
using Microsoft.AspNetCore.Mvc;

namespace EnhancementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDetailsController : Controller
    {
        private readonly IUserDetailsRepository _IUserDetailsRepository;

        public UserDetailsController(IUserDetailsRepository userDetailsRepository) 
        {
            _IUserDetailsRepository = userDetailsRepository;
        }

        /// <summary>
        /// To Fetch User Details
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserDetails")]
        public ActionResult  <List<UserDetails>> GetUserDetails()
        {
            return Ok(_IUserDetailsRepository.GetUserDetails());
        }

        [HttpGet("GetStatusDetails")]
        public ActionResult GetStateusDetails()
        {
            var statusdetails = new List<StatusDetails>
                {
                new StatusDetails {

                    SrNo = 1,

                    Status = "WIP",


                },

                new StatusDetails {

                    SrNo = 2,
                    Status = "Completed",
                },
            };
           


            return Ok(statusdetails);
        }
    }
}
