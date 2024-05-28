using System.Runtime.Serialization;

namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    internal class NoSuchEnrollmentException : Exception
    {
        string _message;
        public NoSuchEnrollmentException()
        {
            _message = "No such enrollment found!";
        }
        public override string Message => _message;

    }
}