using System.Runtime.Serialization;

namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    internal class NoClassAttendanceFoundException : Exception
    {
        string _message;
        public NoClassAttendanceFoundException()
        {
            _message = "No class attendance Found";
        }
        public override string Message => _message;
    }
}