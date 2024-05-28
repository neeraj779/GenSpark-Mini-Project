using System.Runtime.Serialization;

namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    internal class NoSuchCourseException : Exception
    {
        string _message;
        public NoSuchCourseException()
        {
            _message = "No such course found!";
        }
        public override string Message => _message;

    }
}