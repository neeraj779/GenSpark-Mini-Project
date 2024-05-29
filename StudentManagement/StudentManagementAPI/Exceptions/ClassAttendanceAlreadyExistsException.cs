using System.Runtime.Serialization;

namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    internal class ClassAttendanceAlreadyExistsException : Exception
    {
        string _message;
        public ClassAttendanceAlreadyExistsException()
        {
            _message = "You have already marked attendance for this student in this class. Please Update the attendance status if needed.";
        }

        public override string Message => _message;
    }
}