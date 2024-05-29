using System.Runtime.Serialization;

namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    internal class ClassAlreadyExistsException : Exception
    {
        string _message;
        public ClassAlreadyExistsException()
        {
            _message = "Class already exists!";
        }

        public override string Message => _message;

    }
}