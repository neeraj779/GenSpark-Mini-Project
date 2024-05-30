using System.Runtime.Serialization;

namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    public class DuplicateUserNameException : Exception
    {
        string _message;
        public DuplicateUserNameException()
        {
            _message = "Opps! The username is already taken. please try another one.";
        }

        public override string Message => _message;
    }
}