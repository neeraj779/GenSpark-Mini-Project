using System.Runtime.Serialization;

namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    public class NoSuchStudentException : Exception
    {
        string _message;
        public NoSuchStudentException()
        {
            _message = "Uh oh! No such student found!";
        }

        public override string Message => _message;
    }
}