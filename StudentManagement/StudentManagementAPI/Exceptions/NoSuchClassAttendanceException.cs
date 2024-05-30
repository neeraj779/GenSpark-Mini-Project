using System.Runtime.Serialization;

namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    public class NoSuchClassAttendanceException : Exception
    {
        string _message;
        public NoSuchClassAttendanceException()
        {
            _message = "No such class attendance found.";
        }
        public override string Message => _message;
    }
}