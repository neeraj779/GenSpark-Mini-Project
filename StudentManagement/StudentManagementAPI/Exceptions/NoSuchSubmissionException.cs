using System.Runtime.Serialization;

namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    internal class NoSuchSubmissionException : Exception
    {
        string _message;
        public NoSuchSubmissionException()
        {
            _message = "No such submission found!";
        }

        public override string Message => _message;

    }
}