using System.Runtime.Serialization;

namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    internal class NoSuchClassException : Exception
    {
        string _message;
        public NoSuchClassException()
        {
            _message = "No such class found.";
        }

        public override string Message => _message;
    }
}