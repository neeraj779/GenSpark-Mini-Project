using System.Runtime.Serialization;

namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    public class InvalidLoginException : Exception
    {
        string _message;
        public InvalidLoginException()
        {
            _message = "Username or password is incorrect!";
        }

        public override string Message => _message;
    }
}