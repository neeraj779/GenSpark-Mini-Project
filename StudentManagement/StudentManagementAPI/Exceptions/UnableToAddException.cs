using System.Runtime.Serialization;

namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    public class UnableToAddException : Exception
    {
        string _message;
        public UnableToAddException()
        {
            _message = "Uh oh! Something went wrong while adding; please try again.";
        }

        public UnableToAddException(string? message) : base(message)
        {
            _message = message;
        }

        public override string Message => _message;
    }
}