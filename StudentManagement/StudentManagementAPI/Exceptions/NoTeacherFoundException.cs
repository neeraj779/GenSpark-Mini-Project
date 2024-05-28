using System.Runtime.Serialization;

namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    internal class NoTeacherFoundException : Exception
    {
        string _message;
        public NoTeacherFoundException()
        {
            _message = "Uh oh! No teacher found!";
        }

        public override string Message => _message;
    }
}