using System.Runtime.Serialization;

namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    internal class NoSuchTeacherException : Exception
    {
        string _message;
        public NoSuchTeacherException()
        {
            _message = "Uh oh! No such teacher found!";
        }

        public override string Message => _message;
    }
}