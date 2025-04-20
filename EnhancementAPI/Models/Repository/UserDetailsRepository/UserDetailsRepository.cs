using Microsoft.EntityFrameworkCore;
using System;
using static EnhancementAPI.DBContext.EFCoreInMemory;

namespace EnhancementAPI.Models.Repository.UserDetailsRepository
{
    public class UserDetailsRepository:IUserDetailsRepository   
    {
        public UserDetailsRepository() 
        {
            
        }
        public List<UserDetails> GetUserDetails()
        {
            using (var context = new ApiContext())
            {
                var userdetails = new List<UserDetails>
                {
                new UserDetails {

                    Username ="Admin",
                    Password ="Admin123",
                    Role = "Admin"
                },

                new UserDetails
                {
                    Username ="TestUser1",
                    Password ="User123",
                    Role = "User"
                },
                 new UserDetails
                {
                    Username ="TestUser2",
                    Password ="User1234",
                    Role = "User"
                }
                };
                context.UserDetails.AddRange(userdetails);
                context.SaveChanges();
            }
            using (var context = new ApiContext())
            {
                var list = context.UserDetails.ToList();

                List<UserDetails> distinctUser = list
  .GroupBy(p => new { p.Username, p.Role })
  .Select(g => g.First())
  .ToList();

                return distinctUser;
            }
        }
    }
}
