namespace EnhancementAPI.Models.Repository.TaskDetailsRepository
{
    public interface ITaskDetailsRepository
    {
        public List<TaskDetails> GetTaskDetails(string userid);

        public List<TaskDetails> GetDetailsOfSpcificTask(string TaskName);

        public object CreateNewTask(TaskDetails Td);

        public object DeleteTask(Guid Td);

       // public object DeleteUser(int id);

        public List<string> GetStatusList();


        public object AssignNewUserToTask(TaskDetails Td);

    }
}
