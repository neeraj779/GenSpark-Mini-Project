using System.Runtime.Serialization;

namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    internal class UserNotActiveException : Exception
    {
        string _message;
        public UserNotActiveException()
        {
            _message = "Uh oh! User is not active please contact admin!";
        }

        public override string Message => _message;
    }
}