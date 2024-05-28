using System.Runtime.Serialization;

namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    internal class NoStudentFoundException : Exception
    {
        string _message;
        public NoStudentFoundException()
        {
            _message = "Uh oh! No student found!";
        }

        public override string Message => _message;
    }
}