using System.Runtime.Serialization;

namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    internal class NoSuchCourseOfferingException : Exception
    {
        string _message;
        public NoSuchCourseOfferingException()
        {
            _message = "No such course offering found.";
        }
        public override string Message => _message;

    }
}