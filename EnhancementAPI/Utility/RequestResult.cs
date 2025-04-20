using System;

namespace EnhancementAPI.Utility
{


    public class UserStatus
    {
        public bool IsLogin { get; set; } = false;
        public string Msg { get; set; }

    }
    public class RequestResult
    {
        public RequestState State { get; set; }
        public string Msg { get; set; }
        public Object Data { get; set; }
        public Object Results { get; set; }

    }

    public class RequestResult1
    {
        public RequestState State { get; set; }
        public string Msg { get; set; }
        public Object Data { get; set; }
        public Object Results { get; set; }

        public string outtime { get; set; }


    }

    public enum RequestState
    {
        Failed = -1,
        NotAuth = 0,
        Success = 1
    }
    public static class ErrorReponceMessage
    {
        public static string InternalServerError = "There is issue with Internal Server Please try again later";
        public static string RecordAdded = "Record Saved Successfully !!";
        public static string NotFoundError = "Record Not Found !!";
        public static string SelectExecuted = "Total Records Fetched ";
    }
}
