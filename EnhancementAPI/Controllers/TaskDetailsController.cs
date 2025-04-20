using EnhancementAPI.Models;
using EnhancementAPI.Models.Repository.TaskDetailsRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnhancementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskDetailsController : Controller
    {
        private readonly ITaskDetailsRepository _ITaskDetailsRepository;

        public TaskDetailsController(ITaskDetailsRepository taskRepo)
        {
            _ITaskDetailsRepository = taskRepo;
        }
        /// <summary>
        /// /// <summary>
        /// To Get All Task Details
        /// </summary>
        /// <returns>List of Task</returns>
        /// </summary>

        [HttpGet("GetListOfTask/{userId}")]

        public ActionResult<List<TaskDetails>> GetTaskDetails(string userId)
        {
            return Ok(_ITaskDetailsRepository.GetTaskDetails(userId));
        }



        /// <summary>
        /// To Get Specific Task Details
        /// </summary>
        /// <param name="taskName"></param>
        /// <returns>task details</returns>
        [HttpGet("GetSpcificTask/{TaskName}")]
        public ActionResult<List<TaskDetails>> GetDetailsOfSpcificTask(string TaskName)
        {
            return Ok(_ITaskDetailsRepository.GetDetailsOfSpcificTask(TaskName));
        }


        /// <summary>
        /// Create New Task 
        /// </summary>
        /// <param name="detailsTask"></param>
        /// <returns></returns>

        [HttpPost("CreateNewTask")]
        public ActionResult CreateNewTask([FromBody] TaskDetails detailsTask)
        {
            return Ok(_ITaskDetailsRepository.CreateNewTask(detailsTask));
        }

        /// <summary>
        /// Assign Task to New User
        /// </summary>
        /// <param name="taskdetails"></param>
        /// <returns></returns>
        [HttpPost("AssignNewUserToTask")]
        public ActionResult AssignNewUserToTask([FromBody] TaskDetails taskdetails)
        {
            return Ok(_ITaskDetailsRepository.AssignNewUserToTask(taskdetails));
        }

        //[HttpDelete("{id}")]
        //public IActionResult Delete(int id)
        //{
        //    var result = _ITaskDetailsRepository.DeleteUser(id);
        //    //// if (result)
        //    //     return Ok(new { message = "User deleted successfully" });
        //    // else
        //    //     return NotFound(new { message = "User not found" });
        //    return Ok();
        //}

        /// <summary>
        ///  Delete Specific Task
        /// </summary>
        /// <param name="taskdetails"></param>
        /// <returns></returns>
        [HttpPost("DeleteTask")]
        public ActionResult DeleteTask(TaskDetails td)
        {
            Guid TaskId = td.ID;
            return Ok(_ITaskDetailsRepository.DeleteTask(TaskId));
        }


        /// <summary>
        /// To Fetch status list
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetStatusList")]
        public ActionResult GetStatusList()
        {
            return Ok(_ITaskDetailsRepository.GetStatusList());
        }


    }
}
