using System;

namespace InfiStoone.LabClient.Runtime
{
    public class UserFriendException:Exception
    {
        public UserFriendException(string msg):base(msg)
        {

        }

        public string Detail { get; set; }
        public UserFriendException(string msg,string detail) : base(msg)
        {
            this.Detail = detail;
        }
        public int Code { get; set; }
        public UserFriendException(int code,string msg) : base(msg)
        {
            this.Code = code;
        }
    }
}