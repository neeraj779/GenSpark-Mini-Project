using System.Runtime.Serialization;

namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    internal class DuplicateUserException : Exception
    {
        string _message;
        public DuplicateUserException()
        {
            _message = "The user already has an associated account.";
        }

        public override string Message => _message;
    }
}