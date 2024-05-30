namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    public class UnauthorizedUserException : Exception
    {
        string _message;
        public UnauthorizedUserException()
        {
            _message = "Unauthorized user!";
        }

        public override string Message => _message;
    }
}