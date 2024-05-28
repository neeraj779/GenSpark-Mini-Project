using System.Runtime.Serialization;

namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    internal class InvalidRoleException : Exception
    {
        string _message;
        public InvalidRoleException()
        {
            _message = "Invalid role!";
        }

        public override string Message => _message;
    }
}