using System.Runtime.Serialization;

namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    internal class InvalidAttendanceStatusException : Exception
    {
        string _message;
        public InvalidAttendanceStatusException()
        {
            _message = "Invalid attendance status. Please provide a valid attendance status. It can be Present, Absent, Late, or Excused.";
        }

        public override string Message => _message;
    }
}