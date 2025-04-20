using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using static EnhancementAPI.DBContext.EFCoreInMemory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EnhancementAPI.Models.Repository.TaskDetailsRepository
{
    public class TaskDetailsRepository :ITaskDetailsRepository
    {
        public TaskDetailsRepository() 
        
        {
            
        }

        
        public List<TaskDetails> GetTaskDetails(string userid)
        {

            using (var context = new ApiContext())
            {
                var taskdetails = new List<TaskDetails>
                {
                new TaskDetails {

                    SrNo = 1,
                    Title ="Screen Development",
                    AssignedTo = "TestUser1",
                    Description = "Implementation of screen",
                    DueDate = DateTime.Now,
                    Status = "WIP",


                },

                new TaskDetails {

                    SrNo = 2,
                    Title ="API Development",
                    AssignedTo = "TestUser1",
                    Description = "Implementation of API",
                    DueDate = DateTime.Now,
                    Status = "Completed",


                },

                 new TaskDetails {
                     SrNo = 3,
                    Title ="Testing",
                    AssignedTo = "TestUser2",
                    Description = "Unit testing",
                    DueDate = DateTime.Now,
                    Status = "Completed",


                },
                };
                context.TaskDetails.AddRange(taskdetails);
                context.SaveChanges();
            }
            using (var context = new ApiContext())
            {
                var getuserData = context.UserDetails.Where(a => a.Username == userid).ToList();
                if(getuserData.Count>0)
                {
                    if (getuserData[0].Role == "Admin")
                    {
                        var list = context.TaskDetails. ToList();

                        List<TaskDetails> distinctTask = list
                              .GroupBy(p => new { p.SrNo })
                              .Select(g => g.First())
                              .ToList();

                        return distinctTask;
                    }
                    else
                    {
                        var list = context.TaskDetails.Where(a => a.AssignedTo == userid).ToList();
                        List<TaskDetails> distinctTask = list
                             .GroupBy(p => new { p.SrNo })
                             .Select(g => g.First())
                             .ToList();

                        return distinctTask;
                        
                    }
                }
               
            }
            return null;
        }

        
        public List<TaskDetails> GetDetailsOfSpcificTask(string taskName)
        {
            using (var context = new ApiContext())
            {
                var list =  context.TaskDetails.Where(a=>a.Title == taskName.ToString()).ToList();

                return list;
            }
        }


        public object AssignNewUserToTask(TaskDetails Td)
        {
            try
            {
                using (var context = new ApiContext())
                {
                    var list = context.TaskDetails.Where(a => a.SrNo == Td.SrNo).ToList();

                    if (list.Count > 0)
                    {
                        list[0].AssignedTo = Td.AssignedTo;
                        list[0].Description = Td.Description;
                        list[0].Status = Td.Status;
                        list[0].Title = Td.Title;
                    }
                    context.SaveChanges();

                }

                JObject DBReturnResult = new JObject();

                DBReturnResult.Add("Result", "1");

                return DBReturnResult;
            }

            catch (Exception ex)
            {
                JObject DBReturnResult = new JObject();

                DBReturnResult.Add("Result", "-1");

                return DBReturnResult;
            }
               
            
        }


        public bool DeleteUser(int id)
        {
            using (var context = new ApiContext())
            {
                var user = context.TaskDetails.FirstOrDefault(u => u.SrNo == id);
                if (user != null)
                {
                    context.TaskDetails.Remove(user);
                    return true; // successfully deleted
                }
            }

            return false; // user not found
        }
        public object DeleteTask(Guid Td)
        {
            try
            {
                using (var context = new ApiContext())
                {


                    var list = context.TaskDetails.FirstOrDefault(a => a.ID == Td);

                    if (list !=null)
                    {
                        //context.Entry(list).State = EntityState.Deleted;
                        //context.SaveChanges();
                        context.TaskDetails.Remove(list);
                       context.SaveChanges();
                    }


                }

                JObject DBReturnResult = new JObject();

                DBReturnResult.Add("Result", "1");

                return DBReturnResult;
            }

            catch (Exception ex)
            {
                JObject DBReturnResult = new JObject();

                DBReturnResult.Add("Result", "-1");

                return DBReturnResult;
            }


        }


        public List<string> GetStatusList()
        {
            List<string> StatusList = new List<string>();
            StatusList.Add("Pending");
            StatusList.Add("WIP");
            StatusList.Add("Completed");
            return StatusList;
        }
        public object CreateNewTask(TaskDetails Td)
        {
            try
            {
                using (var context = new ApiContext())
                {
                    
                    var TotalCount = context.TaskDetails.ToList();
                    int SrNo = TotalCount.Count + 1;
                    
                    var taskdetails = new TaskDetails
                    {
                        SrNo = SrNo,
                        Title = Td.Title,
                        AssignedTo = Td.AssignedTo,
                        Description = Td.Description,
                        DueDate = DateTime.Now,
                        Status = Td.Status
                    };


                    context.TaskDetails.Add(taskdetails);
                    context.SaveChanges();
                }
                JObject DBReturnResult = new JObject();

                DBReturnResult.Add("Result", "1");

                return DBReturnResult;
            }
            catch (Exception ex)
            {
                JObject DBReturnResult = new JObject();

                DBReturnResult.Add("Result", "-1");

                return DBReturnResult;
            }
        
        }
    }
}