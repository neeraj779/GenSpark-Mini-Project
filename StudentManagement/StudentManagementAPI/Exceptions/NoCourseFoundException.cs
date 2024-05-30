using System.Runtime.Serialization;

namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    public class NoCourseFoundException : Exception
    {
        string _message;
        public NoCourseFoundException()
        {
            _message = "No course found!";
        }
        public override string Message => _message;
    }
}