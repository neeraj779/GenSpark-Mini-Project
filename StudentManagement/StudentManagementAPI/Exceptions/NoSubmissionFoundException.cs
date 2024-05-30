using System.Runtime.Serialization;

namespace StudentManagementAPI.Repositories
{
    [Serializable]
    public class NoSubmissionFoundException : Exception
    {
        string _message;
        public NoSubmissionFoundException()
        {
            _message = "No submission found!";
        }
        public override string Message => base.Message;
    }
}