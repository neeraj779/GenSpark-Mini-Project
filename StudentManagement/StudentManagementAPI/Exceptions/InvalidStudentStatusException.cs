using System.Runtime.Serialization;

namespace StudentManagementAPI.Services
{
    [Serializable]
    public class InvalidStudentStatusException : Exception
    {
        string _message;
        public InvalidStudentStatusException()
        {
            _message = "Invalid Student Status, Please provide a valid status. valid status are Undergraduate, Postgraduate, Alumni, Graduated, DroppedOut, Expelled, Suspended and Transferred";
        }


        public override string Message => _message;
    }
}