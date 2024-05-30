using System.Runtime.Serialization;

namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    public class StudentAlreadyEnrolledException : Exception
    {
        string _message;
        public StudentAlreadyEnrolledException()
        {
            _message = "Student is already enrolled in the course!";
        }

        public override string Message => _message;
    }
}