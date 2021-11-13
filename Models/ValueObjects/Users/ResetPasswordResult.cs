using System.Collections.Generic;

namespace OnlineConsulting.Models.ValueObjects.Users
{
    public class ResetPasswordResult
    {
        public bool IsSucceed { get; set; }
        public List<string> Errors { get; set; }
    }
}
