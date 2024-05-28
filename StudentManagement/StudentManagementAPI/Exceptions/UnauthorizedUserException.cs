using System.Runtime.Serialization;

namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    internal class UnauthorizedUserException : Exception
    {
        string _message;
        public UnauthorizedUserException()
        {
            _message = "Unauthorized user!";
        }

        public override string Message => _message;
    }
}