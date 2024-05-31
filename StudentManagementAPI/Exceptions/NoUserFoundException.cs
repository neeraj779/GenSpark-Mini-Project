namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    public class NoUserFoundException : Exception
    {
        string _message;
        public NoUserFoundException()
        {
            _message = "No user found!";
        }

        public override string Message => _message;
    }
}