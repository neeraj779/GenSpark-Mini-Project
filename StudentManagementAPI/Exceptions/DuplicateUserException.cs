namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    public class DuplicateUserException : Exception
    {
        string _message;
        public DuplicateUserException()
        {
            _message = "The user already has an associated account.";
        }

        public override string Message => _message;
    }
}