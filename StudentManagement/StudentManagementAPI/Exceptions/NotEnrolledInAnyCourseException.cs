using System.Runtime.Serialization;

namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    internal class NotEnrolledInAnyCourseException : Exception
    {
        string _message;
        public NotEnrolledInAnyCourseException()
        {
            _message = "You are not enrolled in any course.";
        }

        public override string Message => _message;
    }
}