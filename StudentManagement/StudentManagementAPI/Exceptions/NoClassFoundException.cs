namespace StudentManagementAPI.Exceptions
{
    [Serializable]
    public class NoClassFoundException : Exception
    {
        string _message;
        public NoClassFoundException()
        {
            _message = "No class found!";
        }
        public override string Message => _message;
    }
}