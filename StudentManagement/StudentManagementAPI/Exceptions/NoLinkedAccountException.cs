using System.Runtime.Serialization;

namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    public class NoLinkedAccountException : Exception
    {
        string _message;
        public NoLinkedAccountException()
        {
            _message = "Opps it seems you don't have a linked User Account. Please link your account first and try again.";
        }
        public override string Message => _message;
    }
}