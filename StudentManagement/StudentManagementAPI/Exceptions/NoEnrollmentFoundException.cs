using System.Runtime.Serialization;

namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    internal class NoEnrollmentFoundException : Exception
    {
        string _message;
        public NoEnrollmentFoundException()
        {
            _message = "No enrollment found!";
        }
        public override string Message => _message;
    }
}